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
        public Vector3 upperforce;

        [Header("Levitation params")] private RaycastHit ground;
        [Range(0, 10)] public float GroundDistance;

        [Range(0, 10)] public float levitationHeight;

        [Header("Test params")] 
        
        public WheelCollider FrontWeelR, FrontWeell;
        //Swap right
        private bool rightSwipe;

        private void Update()
        {
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            _swipeCOntroll = gameObject.AddComponent<SwipeControll>(); //GetComponent<SwipeControll>();
            _swipeCOntroll.leftSwipe = RotateLeft;
            _swipeCOntroll.RightSwipe = RotateRight;
            _swipeCOntroll.UpSwipe = AddNitro;
            _swipeCOntroll.DownSwipe = OfNitro;
        }


        private void FixedUpdate()
        {
            MooveForces();
            SpeedChanger();
        //    addLevitation();
       //   if (onAGround) Levitation();
            ParamsCheck();
        }

        void addLevitation()
        {
            if (transform.position.y<0)
            {
                rb.AddForce(upperforce,ForceMode.VelocityChange);
            }
        }

        void MooveForces()
        {
            if (start && CurSpeed < MaxSpeed)
            {
              //  rb.AddRelativeForce(Vector3.forward * AccelerationSpeed, ForceMode.Acceleration);
            //  rb.AddRelativeForce(Vector3.forward * AccelerationSpeed, ForceMode.Force);
              FrontWeelR.motorTorque =  AccelerationSpeed*2;
              FrontWeell.motorTorque =  AccelerationSpeed*2;
            }

            if (CurSpeed > MaxSpeed)
            {
                FrontWeelR.motorTorque =  -AccelerationSpeed*2;
                FrontWeell.motorTorque =  -AccelerationSpeed*2;  
            }

            if (_onNitro)
            {
                rb.AddRelativeForce(Vector3.forward * BoostAcceleration, ForceMode.Acceleration);
            }


            if (transform.position.y < -5)
            {
                _carCntrl.DeadEnd();
            }
        }

        void ParamsCheck()
        {
            //На случай проверки земли
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0),
                transform.TransformDirection(Vector3.down), out hit, 2.5f, 1))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
                if (hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>()!=null)
                {
                    Debug.Log("find");
                    hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>().PingDestroy();
                }

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

        void Levitation()
        {
            if (Physics.Raycast(transform.position, -transform.up, out ground))
            {
                GroundDistance = ground.distance;
            }

            float delta = Mathf.Abs(levitationHeight - GroundDistance);
            delta = Mathf.Pow(delta, 2);

            if (GroundDistance < levitationHeight)
            {
                rb.AddForce(transform.up * 2 * delta, ForceMode.Impulse);
            }

            if (GroundDistance > levitationHeight - 0.04f)
            {
                rb.AddForce(-transform.up * 2 * delta, ForceMode.Impulse);
            }
            else
            {
               rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
               // rb.velocity = Vector3.zero;
            }

            Debug.Log(rb.velocity);
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
rb.AddRelativeForce(Vector3.forward*3, ForceMode.Impulse);
            /*if (rightSwipe)
            {
                switch (mooveType)
                {
                    case 1:
                        rb.velocity = new Vector3(-2, 0, -rb.velocity.x);
                        break;
                    case 2:
                        rb.velocity = new Vector3(rb.velocity.z, 0, axis.y / 90);
                        break;
                    case 3:
                        rb.velocity = new Vector3( axis.y / 90, 0, -rb.velocity.x);
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
            }*/

            isRotating = false;
        }
    }
}