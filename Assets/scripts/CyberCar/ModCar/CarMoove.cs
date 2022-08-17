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
        public CarCntrl _carCntrl;
        public CanvasView cview;
        public float doubleClickTime = .2f, lastClickTime, longpress = 1f;
        float speedBoost;
        public bool _onNitro;
        private float StartTime, EndTime;

        void Start()
        {
            _carCntrl = GetComponent<CarCntrl>();
            StartTime = 0;
            EndTime = 0;
            cview = CanvasView.Instance;
            cview.ShowNitroBalance(_carCntrl.GameManager.NitroBonus );
        }

        private void Update()
        {

            if (_carCntrl.GameManager.NitroBonus > 0 && _onNitro)
            {
                _carCntrl.GameManager.NitroBonus -= Time.deltaTime *156;
                speedBoost = _carCntrl.speedBoost;
                _onNitro = true;
                cview.ShowNitroBalance(_carCntrl.GameManager.NitroBonus );
            }
            else
            {
                cview.ShowNitroEffect(false);
                _onNitro = false;
                speedBoost = 1;
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
            else if (!_onNitro && Speed > _carCntrl.MaxSpeed)
            {
                Speed = _carCntrl.MaxSpeed;
            }

            if (Speed < _carCntrl.MaxNitroSpeed && isStarted && _onNitro)
            {
                Speed += speedBoost;
            }


            if (cview && isStarted) cview.SetSpeed((int) Speed);
        }

        public void NitroCar()
        {
            _onNitro = true;
        }
        public void StopNitroCar()
        {
            _onNitro = false;
        }

        public void TurnCar()
        {
           // Handheld.Vibrate();
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
                    if (front) transform.Translate(Vector3.forward * (Speed * Time.fixedDeltaTime * speedBoost));
                    else transform.Translate(Vector3.forward * (Speed * Time.fixedDeltaTime * speedBoost));
                }
            }
        }
    }
}