using System;
using System.Collections;
using System.Collections.Generic;
using CyberCar;
using CyberCar.ModCanvas;
using UnityEngine;

namespace ModCar
{
    public class DriftMoove : MoveBasic
    {
        [SerializeField] private bool isRotating;
        private SwipeControll _swipeCOntroll;
        [Header("Engine move params")] public float RotationSpeed;
        public float Inertion;
        public float BoostAcceleration;
        [SerializeField] private bool _onBroke;
        [Header("inner params")] public int mooveType;
        [Header("Links params")] public CarCntrl _carCntrl;
        public CanvasView cview;
        public Vector3 upperforce;
        [Header("Levitation params")] private RaycastHit ground;
        [Range(0, 10)] public float GroundDistance;
        [Range(0, 10)] public float levitationHeight;
        WheelFrictionCurve DefaultFC;
        WheelFrictionCurve DefaultSFC;

        //Swap right
        private bool rightSwipe;
        [Header("New moove param")] public List<WheelMove> axleInfos;
        public float maxMotorTorque;
        public float maxSteeringAngle;
        [SerializeField] private Rigidbody body;
        private float v, h;
        public VariableJoystick Joystick;
        private void Start()
        {
            body = GetComponent<Rigidbody>();
            _swipeCOntroll = gameObject.AddComponent<SwipeControll>(); //GetComponent<SwipeControll>();
            _swipeCOntroll.leftSwipe = RotateLeft;
            _swipeCOntroll.RightSwipe = RotateRight;
            _swipeCOntroll.UpSwipe = AddNitro;
            _swipeCOntroll.DownSwipe += OfNitro;
            _swipeCOntroll.DownSwipe += brokeDrossel;
            for (int a = 0; a < axleInfos.Count; a++)
            {
                axleInfos[a].leftWheel.ConfigureVehicleSubsteps(5, 12, 15);
                axleInfos[a].rightWheel.ConfigureVehicleSubsteps(5, 12, 15);
                
            }
            setWheelParams(axleInfos[1].rightWheel);
            setWheelParams(axleInfos[1].leftWheel);
            Joystick = FindObjectOfType<VariableJoystick>();

           
        }

        void setWheelParams(WheelCollider wheel)
        {
            DefaultFC = wheel.forwardFriction;
          //  wheel.forceAppPointDistance = 1.24f;
           // wheel.mass = 20;
            wheel.wheelDampingRate = 0.25f;
            wheel.suspensionDistance = 0.3f;
            WheelFrictionCurve FC = wheel.forwardFriction;
            FC.extremumSlip = 200;
            /*FC.extremumValue = 1;
            FC.asymptoteSlip = 0.8f;
            FC.asymptoteValue = 0.5f;*/
            wheel.forwardFriction =FC;

            WheelFrictionCurve SFC = wheel.sidewaysFriction;
            SFC.extremumSlip = 0.11f;
            /*SFC.extremumValue = 1;
            SFC.asymptoteSlip = 0.5f;
            SFC.asymptoteValue = 0.75f;*/
            wheel.sidewaysFriction = SFC;


        }

        private void FixedUpdate()
        {
            Moove();
            SpeedChanger();
          //  ParamsCheck();
            CheckInfo();
        }

        void brokeDrossel()
        {
            StartCoroutine(BrokeDrive());
        }

        IEnumerator BrokeDrive()
        {
            _onBroke = true;
            float t = 0;
            while (t < 1)
            {
                yield return
                    body.drag = 0.3f;
                t += Time.deltaTime;
            }

            body.drag = 0;
            _onBroke = false;
        }


        void ParamsCheck()
        {
            switch (mooveType)
            {
                case 1:
                    CurSpeed = body.velocity.z;
                    break;
                case 2:
                    CurSpeed = body.velocity.x;
                    break;
                case 3:
                    CurSpeed = -body.velocity.z;
                    break;
                case 4:
                    CurSpeed = -body.velocity.x;
                    break;
            }
        }


        public void RotateRight()
        {
            if (!isRotating && OnAGround)
            {
                rightSwipe = true;
                // StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
                StartCoroutine(DoRotation(new Vector3(0, 40, 0)));
            }
        }

        public void RotateLeft()
        {
            if (!isRotating && OnAGround)
            {
                rightSwipe = false;
                // StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
                StartCoroutine(DoRotation(new Vector3(0, -40, 0)));
            }
        }

        public void AddNitro()
        {
            Nitro(true);
        }

        public void OfNitro()
        {
            Nitro(false);
        }

        public override void Nitro(bool _onNitro)
        {
            onNitro = _onNitro;
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

        IEnumerator DoRotation(Vector3 axis)
        {
           
            foreach (WheelMove axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.CalculateAndApplySteering(axis.y, maxSteeringAngle, axleInfos);
                }
            }
            yield return new WaitForSeconds(1);
            foreach (WheelMove axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.CalculateAndApplySteering(0, maxSteeringAngle, axleInfos);
                }
            }
        }

        IEnumerator DoRotation(float _speed, float amount, Vector3 axis)
        {
            body.isKinematic = true;
            isRotating = true;
            float rot = 0f;
            while (rot < amount)
            {
                yield return null;
                float delta = Mathf.Min(_speed * Time.deltaTime, amount - rot);
                transform.RotateAround(transform.position, axis, delta);
                rot += delta;
            }

            body.isKinematic = false;
            body.AddRelativeForce(Vector3.forward * 3, ForceMode.Impulse);
            isRotating = false;
        }


        public override void Moove()
        {
            h = Joystick.Horizontal;
            //  if (start) body.isKinematic = true;
            foreach (WheelMove axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.CalculateAndApplySteering(h, maxSteeringAngle, axleInfos);
                }
            }
            float motor = maxMotorTorque;
            if (onNitro && CurSpeed < MaxNitroSpeed)
            {
                body.AddRelativeForce(Vector3.forward * 10, ForceMode.Acceleration);
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



        public override void BrakeTorque(bool inBrake)
        {
            throw new NotImplementedException();
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
                body.AddForce(transform.up * 2 * delta, ForceMode.Impulse);
            }

            if (GroundDistance > levitationHeight - 0.04f)
            {
                body.AddForce(-transform.up * 2 * delta, ForceMode.Impulse);
            }
            else
            {
                body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);
                // rb.velocity = Vector3.zero;
            }

            Debug.Log(body.velocity);
        }
    }
}