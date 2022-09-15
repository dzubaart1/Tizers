using System;
using System.Collections;
using CyberCar.ModCanvas;
using UnityEngine;

namespace CyberCar
{
    public class CarMoove : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private bool isRotating;
        private SwipeControll _swipeCOntroll;
        [Header("Engine move params")] public float CurSpeed;
        public float AccelerationSpeed = 4;
        public float MaxSpeed;
        public float RotationSpeed;
        public float Inertion;
        public float BoostAcceleration;
        [Header("inner params")] public int mooveType;
        public bool start;
        public bool _onNitro;
        private bool onAGround;
        [Header("Links params")] public CarCntrl _carCntrl;
        public CanvasView cview;


        //Swap right
        private bool rightSwipe;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            _swipeCOntroll = gameObject.AddComponent<SwipeControll>();//GetComponent<SwipeControll>();
            _swipeCOntroll.leftSwipe = RotateLeft;
            _swipeCOntroll.RightSwipe = RotateRight;
            _swipeCOntroll.UpSwipe = AddNitro;
            _swipeCOntroll.DownSwipe = OfNitro;
        }


        private void FixedUpdate()
        {
            if (start && CurSpeed < MaxSpeed)
            {
                rb.AddRelativeForce(Vector3.forward * AccelerationSpeed, ForceMode.Acceleration);
            }

            if (_onNitro)
            {
                rb.AddRelativeForce(Vector3.forward * BoostAcceleration, ForceMode.Acceleration);
            }

            if (transform.position.y < -5)
            {
                _carCntrl.DeadEnd();
            }

            SpeedChanger();

            //На случай проверки земли
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


        /*public void Rotate(bool right)
        {
            rightSwipe = right;
            if (right)
            {
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
            }
            else
            {
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
            }
        }*/

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

        public void AddNitro()
        {
            _onNitro = true;
        }

        public void OfNitro()
        {
            _onNitro = false;
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

        IEnumerator DoRotation(float _speed, float amount, Vector3 axis)
        {
            isRotating = true;
            float rot = 0f;
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

            isRotating = false;
        }
    }
}