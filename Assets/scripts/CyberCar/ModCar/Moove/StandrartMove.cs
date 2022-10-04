using System;
using System.Collections.Generic;
using CyberCar;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ModCar
{
    [RequireComponent(typeof(Rigidbody))]
    public class StandrartMove : MoveBasic
    {
        public VariableJoystick Joystick;
        public List<WheelMove> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;
        private float v, h;
        private Rigidbody body;
       

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

     
        public void FixedUpdate()
        {
            // v = Joystick.Vertical;
            //   v = 1;
            Moove();
            CheckInfo();
        }

        public override void BrakeTorque(bool inBrake)
        {
            onBrake = inBrake;
        }

        public override void Nitro(bool _onNitro)
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

        public override void Moove()
        {
            if (start) body.isKinematic = true;
            
              h = Joystick.Horizontal/2;
            float motor = maxMotorTorque; //*v;
            if (onNitro && CurSpeed < MaxNitroSpeed)
            {
                body.AddRelativeForce(Vector3.forward * 10, ForceMode.Acceleration);
            }

            foreach (WheelMove axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.CalculateAndApplySteering(h, maxSteeringAngle, axleInfos);
                }
            }

            CurSpeed = body.velocity.magnitude;
            if (!onBrake)
            {
                foreach (WheelMove axleInfo in axleInfos)
                {
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

        }
    }
}