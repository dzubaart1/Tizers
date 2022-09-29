using System;
using System.Collections.Generic;
using CyberCar;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TestsScript
{
    [RequireComponent(typeof(Rigidbody))]
    public class SimpleCarController : MonoBehaviour
    {
        public VariableJoystick Joystick;
        public List<WheelMove> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;
        private float v, h;
        private Rigidbody body;
        [SerializeField] private float CurSpeed;
        public float MaxSpeed;
        public float MaxNitroSpeed;
        public float maxBrake = 50;
        public bool onBrake;
        public bool onNitro;
        public bool OnAGround;

        private void Start()
        {
            Joystick = FindObjectOfType<VariableJoystick>();
            body = GetComponent<Rigidbody>();
            body.constraints = RigidbodyConstraints.None;
            for (int a = 0; a < axleInfos.Count; a++)
            {
                axleInfos[a].leftWheel.ConfigureVehicleSubsteps(5, 12, 15);
                axleInfos[a].rightWheel.ConfigureVehicleSubsteps(5, 12, 15);
            }
        }

        void Checkers()
        {
            RaycastHit hit;
            RaycastHit hitGround;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) , out hitGround,
                1,
                1))
            {
                OnAGround = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.blue);
                OnAGround = false;
            }

            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 1000, out hit,
                Mathf.Infinity,
                1))
            {
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
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
        }


        public void FixedUpdate()
        {
            // v = Joystick.Vertical;
            //   v = 1;
            h = Joystick.Horizontal;
            float motor = maxMotorTorque; //*v;
            if (onNitro && CurSpeed < MaxNitroSpeed)
            {
                body.AddRelativeForce(Vector3.forward * 10, ForceMode.Acceleration);
            }


            CurSpeed = body.velocity.magnitude;
            if (!onBrake)
            {
                foreach (WheelMove axleInfo in axleInfos)
                {
                    if (axleInfo.steering)
                    {
                        axleInfo.CalculateAndApplySteering(h, maxSteeringAngle, axleInfos);
                    }

                    if (axleInfo.motor && CurSpeed < MaxSpeed)
                    {
                        axleInfo.leftWheel.motorTorque = motor;
                        axleInfo.rightWheel.motorTorque = motor;
                    }

                    if (!onNitro)
                    {
                        if (CurSpeed > MaxSpeed)
                        {
                            axleInfo.leftWheel.brakeTorque = maxBrake;
                            axleInfo.rightWheel.brakeTorque = maxBrake;
                        }
                        else
                        {
                            axleInfo.leftWheel.brakeTorque = 0;
                            axleInfo.rightWheel.brakeTorque = 0;
                        }
                    }
                    else
                    {
                        if (CurSpeed > MaxNitroSpeed)
                        {
                            axleInfo.leftWheel.brakeTorque = maxBrake;
                            axleInfo.rightWheel.brakeTorque = maxBrake;
                        }
                        else
                        {
                            axleInfo.leftWheel.brakeTorque = 0;
                            axleInfo.rightWheel.brakeTorque = 0;
                        }
                    }

                    axleInfo.ApplyLocalPositionToVisuals();
                    axleInfo.CalculateAndApplyAntiRollForce(body);
                }
            }
            else
            {
                foreach (WheelMove axleInfo in axleInfos)
                {
                    axleInfo.leftWheel.brakeTorque = maxBrake;
                    axleInfo.rightWheel.brakeTorque = maxBrake;
                }
            }

            Checkers();
        }

        public void BrakeTorque(bool inBrake)
        {
            onBrake = inBrake;
        }

        public void Nitro(bool _onNitro)
        {
            onNitro = _onNitro;
            if (_onNitro == false)
            {
                foreach (WheelMove axleInfo in axleInfos)
                {
                    axleInfo.leftWheel.brakeTorque = maxBrake;
                    axleInfo.rightWheel.brakeTorque = maxBrake;
                }
            }
        }
    }
}