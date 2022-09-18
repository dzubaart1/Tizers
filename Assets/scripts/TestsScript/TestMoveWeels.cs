using System;
using System.Collections;
using UnityEngine;

namespace TestsScript
{
    public class TestMoveWeels : MonoBehaviour
    {
        [SerializeField] private bool isRotating;
        private Rigidbody rb;
        [Header("inner params")] public int mooveType;
        public float RotationSpeed;
        public float Inertion;
        private bool rightSwipe;
        public bool onAGround = true;
        public float CurSpeed;
        
        
        [Header("WheelsParams")] 
        public WheelCollider front_driverCol, front_passColl;
        public WheelCollider back_driverCol, back_passCol;
        public Transform frontDriver, frontPass;
        public Transform backDriver, backPass;
        public float _steerAngle = 25;
        public float _motorForce = 1500f;
        public float steerAngl;
        public bool onTransUpdate;
        private float h, v;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void SpeedChanger()
        {
            int y = (int) Math.Ceiling(transform.localEulerAngles.y);
            if (rightSwipe)
            {
                if (y == 1 || y > 271 && y <= 361)
                {
                    mooveType = 1;
                }

                if (y > 1 && y <= 91)
                {
                    mooveType = 2;
                }

                if (y > 91 && y <= 181)
                {
                    mooveType = 3;
                }

                if (y > 181 && y <= 271)
                {
                    mooveType = 4;
                }
            }
            else
            {
                if (y >= 270 && y < 360)
                {
                    mooveType = 4;
                }

                if (y < 270 && y >= 180)
                {
                    mooveType = 3;
                }

                if (y < 180 && y >= 90)
                {
                    mooveType = 2;
                }

                if (y == 0 || y >= 0 && y < 90)
                {
                    mooveType = 1;
                }
            }
        }

        private void Update()
        {
            Inputs();
        }

        private void FixedUpdate()
        {
            
            Drive();
            SteerCar();
            SpeedChanger();
            Checkers();
            
            if (onTransUpdate)
            {
                UpdateWheelPos(front_driverCol, frontDriver);
                UpdateWheelPos(front_passColl, frontPass);
                UpdateWheelPos(back_driverCol, backDriver);
                UpdateWheelPos(back_passCol, backPass);
            }
          
        }

        void Checkers()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0),
                transform.TransformDirection(Vector3.down), out hit, 2.5f, 1))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
                onAGround = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 5, Color.yellow);
                onAGround = false;
            }

            switch (mooveType)
            {
                case 1:
                    CurSpeed = rb.velocity.z;
                    break;
                case 2:
                    CurSpeed = rb.velocity.x;
                    break;
                case 3:
                    CurSpeed = -rb.velocity.z;
                    break;
                case 4:
                    CurSpeed = -rb.velocity.x;
                    break;
            }
        }

        void Inputs()
        {
            if (Input.GetKeyUp(KeyCode.D)&& !isRotating)
            {
                RotateRight();
            }
            if (Input.GetKeyUp(KeyCode.A)&& !isRotating)
            {
                RotateLeft();
            }

            /*h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");*/
        }
        public void RotateRight()
        {
            if (!isRotating && onAGround)
            {
                rightSwipe = true;
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
            }
        }

        public void RotateLeft()
        {
            if (!isRotating && onAGround)
            {
                rightSwipe = false;
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
            }
        }
        void Drive()
        {
            v = 0.5f;
            back_driverCol.motorTorque = v * _motorForce;
            back_passCol.motorTorque = v * _motorForce;
        }

        void SteerCar()
        {
            steerAngl = _steerAngle * h;
            front_driverCol.steerAngle = steerAngl;
            front_passColl.steerAngle = steerAngl;
        }

        void UpdateWheelPos(WheelCollider col, Transform t)
        {
            Vector3 pos = t.position;
            Quaternion rot = t.rotation;
            col.GetWorldPose(out pos, out rot);
            t.position = pos;
            t.rotation = rot;
        }
        
        IEnumerator DoRotation(float _speed, float amount, Vector3 axis)
        {
            isRotating = true;
            float rot = 0f;
            if (axis.y > 0)
            {
                h = 1;
            }
            else
            {
                h = -1;
            }

            while (rot < amount)
            {
                yield return null;
                float delta = Mathf.Min(_speed * Time.deltaTime, amount - rot);
                transform.RotateAround(transform.position, axis, delta);
                rot += delta;
            }

            if (rightSwipe)
            {
                switch (mooveType)
                {
                    case 1:
                        rb.velocity = new Vector3(-2, 0, -rb.velocity.x);
                        break;
                    case 2:
                        rb.velocity = new Vector3(rb.velocity.z, 0, 2 * axis.y / 90);
                        break;
                    case 3:
                        rb.velocity = new Vector3(2 * axis.y / 90, 0, -rb.velocity.x);
                        break;
                    case 4:
                        rb.velocity = new Vector3(rb.velocity.z, 0, -2);
                        break;
                }
            }
            else
            {
                switch (mooveType)
                {
                    case 1:
                        rb.velocity = new Vector3(-Inertion, 0, rb.velocity.x);
                        break;
                    case 2:
                        rb.velocity = new Vector3(-rb.velocity.z, 0, Inertion);
                        break;
                    case 3:
                        rb.velocity = new Vector3(-Inertion, 0, rb.velocity.x);
                        break;
                    case 4:
                        rb.velocity = new Vector3(-rb.velocity.z, 0, Inertion);
                        break;
                }
            }

            h = 0;
            isRotating = false;
        }
    }
}