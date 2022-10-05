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
        public float BoostAcceleration;
        [SerializeField] private bool _onBroke;
        [Header("inner params")] public int mooveType;
        [Header("Links params")] public CarCntrl _carCntrl;
        public CanvasView cview;
        [Header("Levitation params")] private RaycastHit ground;
        [Range(0, 10)] public float GroundDistance;
        [Range(0, 10)] public float levitationHeight;
        WheelFrictionCurve DefaultFC;
        WheelFrictionCurve DrifttFC;
        WheelFrictionCurve DefaultSFC;
        WheelFrictionCurve DriftSFC;

        //Swap right
        private bool rightSwipe;
        [SerializeField] private Rigidbody body;
        private float v, h;
        public VariableJoystick Joystick;

        public WheelCollider FrontWeelR, FrontWeell;

        private void Start()
        {
            body = GetComponent<Rigidbody>();
            DriftWheels.SetActive(true);
            _swipeCOntroll = gameObject.AddComponent<SwipeControll>();
            _swipeCOntroll.leftSwipe = RotateLeft;
            _swipeCOntroll.RightSwipe = RotateRight;
            _swipeCOntroll.UpSwipe = AddNitro;
            _swipeCOntroll.DownSwipe += OfNitro;
            _swipeCOntroll.DownSwipe += brokeDrossel;
            Joystick = FindObjectOfType<VariableJoystick>();
            Joystick.enabled = false;
        }


        private void FixedUpdate()
        {
            Moove();
            SpeedChanger();
            ParamsCheck();
            CheckInfo();
        }

        #region Moove Logic

        public override void Moove()
        {
            CurSpeed = body.velocity.magnitude;
            if (start && CurSpeed < MaxSpeed && !_onBroke)
            {
                //  rb.AddRelativeForce(Vector3.forward * AccelerationSpeed, ForceMode.Acceleration);
                //  rb.AddRelativeForce(Vector3.forward * AccelerationSpeed, ForceMode.Force);
                FrontWeelR.motorTorque = AccelerationSpeed * 2;
                FrontWeell.motorTorque = AccelerationSpeed * 2;
            }

            if (CurSpeed > MaxSpeed)
            {
                FrontWeelR.motorTorque = -AccelerationSpeed * 2;
                FrontWeell.motorTorque = -AccelerationSpeed * 2;
            }

            if (onNitro)
            {
                body.AddRelativeForce(Vector3.forward * BoostAcceleration, ForceMode.Acceleration);
            }


            if (transform.position.y < -5)
            {
                _carCntrl.DeadEnd();
            }
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

        #endregion


        #region AddParams

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

        #endregion

        #region Controll Logic

        public void RotateRight()
        {
            if (!isRotating && OnAGround)
            {
                rightSwipe = true;
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
            }
        }

        public void RotateLeft()
        {
            if (!isRotating && OnAGround)
            {
                rightSwipe = false;
                StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
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

            isRotating = false;
            body.AddRelativeForce(Vector3.forward * 3, ForceMode.Impulse);
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

        #endregion
    }
}