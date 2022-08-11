using System;
using System.Collections;
using CyberCar.ModCanvas;
using UnityEngine;

namespace CyberCar
{
    public class CarMoove : MonoBehaviour
    {
        public bool isStarted;
        public float Speed = 5;
        bool front;
        private CarCntrl _carCntrl;
        public CanvasView cview;
        public float doubleClickTime = .2f, lastClickTime, longpress = 1f;
        int speedBoost;
        private bool _onNitro;
        private float StartTime, EndTime;

        void Start()
        {
            _carCntrl = GetComponent<CarCntrl>();
            StartTime = 0;
            EndTime = 0;
            cview.ShowNitroBalance(_carCntrl.GameManager.NitroBonus );
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && isStarted)
            {
                StartTime = Time.time;
            }
            if (Input.GetMouseButtonUp(0) && isStarted)
            {
              EndTime = Time.time;
              StartTime = 0;
              _onNitro = false;
              cview.ShowNitroEffect(false);
            }

            if (Time.time - StartTime >longpress && StartTime!=0)
            {
                if (_carCntrl.GameManager.NitroBonus > 0)
                {
                    _carCntrl.GameManager.NitroBonus -= Time.deltaTime *60;
                    speedBoost = _carCntrl.speedBoost;
                    _onNitro = true;
                    cview.ShowNitroBalance(_carCntrl.GameManager.NitroBonus );
                    cview.ShowNitroEffect(true);
                }
                else
                {
                    cview.ShowNitroEffect(false);
                    _onNitro = false;
                    speedBoost = 0;
                    if (Speed > _carCntrl.MaxSpeed) Speed = _carCntrl.MaxSpeed;
                }
            }


            if (Input.GetMouseButtonDown(0) && isStarted)
            {
                float timeSinceLastClick = Time.time - lastClickTime;

                if (timeSinceLastClick <= doubleClickTime)
                {
                    TurnCar();
                }
                /*else if (timeSinceLastClick >= longpress)
                {
                    if (_carCntrl.GameManager.NitroBonus > 0)
                    {
                        _carCntrl.GameManager.NitroBonus -= (int) Time.deltaTime * 5;
                        speedBoost = _carCntrl.speedBoost;
                        _onNitro = true;
                        Debug.Log("longPress");
                    }
                    else
                    {
                        _onNitro = false;
                        speedBoost = 0;
                    }
                }*/

                lastClickTime = Time.time;
            }


            if (transform.position.y < -5)
            {
                isStarted = false;
                _carCntrl.DeadEnd();
            }

            if (Speed < _carCntrl.MaxSpeed && isStarted)
            {
                Speed += Time.deltaTime * 2;
            }

            if (Speed < _carCntrl.MaxNitroSpeed && isStarted && _onNitro)
            {
                Speed += speedBoost;
            }


            if (cview && isStarted) cview.SetSpeed((int) Speed);
        }

        public void TurnCar()
        {
            Handheld.Vibrate();
            if (!front)
            {
                _carCntrl.Anim.SetTrigger("right");
                if (Speed - 2 > 10) Speed -= 2;
            }
            else
            {
                _carCntrl.Anim.SetTrigger("front");
                if (Speed - 2 > 10) Speed -= 2;
            }

            front = !front;
        }


        void FixedUpdate()
        {
            if (isStarted)
            {
                if (_carCntrl.inGame)
                {
                    if (front) transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
                    else transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
                }
            }
        }
    }
}