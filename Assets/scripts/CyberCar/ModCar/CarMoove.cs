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
        void Start()
        {
            _carCntrl = GetComponent<CarCntrl>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A) && !isStarted) isStarted = true;
           

            if (Input.GetKeyUp(KeyCode.Space))
            {
                TurnCar();
            }

            if (transform.position.y < -5)
            {
                isStarted = false;
                _carCntrl.DeadEnd();

            }

            if (Speed < _carCntrl.MaxSpeed && isStarted)
            {
                Speed += Time.deltaTime*2;
            }
            if(cview && isStarted) cview.SetSpeed((int)Speed);
        }

        public void TurnCar()
        {
            if (!front)
            {
                _carCntrl.Anim.SetTrigger("right");
                if(Speed-2 >10) Speed -= 2;
            }
            else
            {
                _carCntrl.Anim.SetTrigger("front");
                if(Speed-2 >10) Speed -= 2;
                
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