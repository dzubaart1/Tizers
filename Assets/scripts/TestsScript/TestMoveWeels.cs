using System;
using System.Collections;
using CyberCar;
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
        public float MaxSpeed;
        
        [Header("WheelsParams")] 
        public float _steerAngle = 25;
        public WheelCollider front_driverCol, front_passColl;
        public WheelCollider back_driverCol, back_passCol;
        public Transform frontDriver, frontPass;
        public Transform backDriver, backPass;
        public float _motorForce = 1500f;
        public float steerAngl;
        public bool onTransUpdate;
        private float h, v;
        public float brokeForce;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.None;
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
            RaycastHit hit = default;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
            if (hit.transform.parent && hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>() != null)
            {
                RoadPlaneCntrl road = hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>();
                road.PingDestroy();
            }
            else
            {
                if (hit.transform.gameObject.GetComponent<RoadPlaneCntrl>() != null)
                {
                    RoadPlaneCntrl road = hit.transform.gameObject.GetComponent<RoadPlaneCntrl>();

                    road.PingDestroy();
                }
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
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }
      
        void Drive()
        {
           
            
            if ( CurSpeed < MaxSpeed)
            {
                Unbroke();
                front_driverCol.motorTorque = v * _motorForce;
                front_passColl.motorTorque = v * _motorForce;
            }
            

            if (CurSpeed > MaxSpeed)
            {
                Broke();
            }

        }

        void Broke()
        {
            back_driverCol.brakeTorque  =brokeForce;
            back_passCol.brakeTorque  = brokeForce;
            front_driverCol.brakeTorque  = brokeForce;
            front_passColl.brakeTorque  = brokeForce;
        }

        void Unbroke()
        {
                back_driverCol.brakeTorque  =0;
                back_passCol.brakeTorque  = 0;
                front_driverCol.brakeTorque = 0;
                front_passColl.brakeTorque  = 0;
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
        
       
    }
}