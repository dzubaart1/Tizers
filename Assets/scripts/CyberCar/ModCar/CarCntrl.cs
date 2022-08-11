using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar.Bonuses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class CarCntrl: MonoBehaviour
    {
        public float MaxSpeed;
        public float MaxNitroSpeed;
        public CarGameManager GameManager;
        public GameObject ExplodeCar;
        public bool inGame =true;
        public Rigidbody _rb;
        private CarMoove _moove;
        public Animator Anim;
        public CarModelCntrl CarModel;
        private float curspeed;
        private BonusEfect curBonus;
        public int speedBoost;
        void Start()
        {
            List<CarParams> CarsObjs = Resources.LoadAll<CarParams>("CustomCars").ToList();
            CarModel = Instantiate(CarsObjs[0].CarModel,transform);
            CarModel.transform.localPosition = new Vector3(0, 0.241f, 0);
            CarModel.setData(CarsObjs[0].BackLights[1], CarsObjs[0].TexturesList[2]);
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            _moove = GetComponent<CarMoove>();
            Anim = GetComponent<Animator>();
        }
        public void StartGame()
        {
            _moove.isStarted = true;
            _rb.isKinematic = false;
        }
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag == "border")
            {
                DeadEnd();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "bonus" && curBonus==null)
            {
                BonusCntrl bonus =other.GetComponent<BonusCntrl>();
                bonus.GetBonus();
                if (bonus.Effect.SpeedBonus)
                {
                    StartCoroutine(SpeedBonus(bonus.Effect));
                } 
                if (bonus.Effect.AddScore)
                {
                    GameManager.AddScore(bonus.Effect.ScoreCount);
                }
                if (bonus.Effect.NitroBonus)
                {
                    GameManager.NitroBonus+=bonus.Effect.NitroCount;
                    GameManager._CarCanvas.UpdateNitroView();
                }

            }
        }

        public void DeadEnd()
        {
            _rb.isKinematic = true;
            ExplodeCar.SetActive(true);
            GameManager.isDie();
            
            
        }

       

      
       IEnumerator SpeedBonus(BonusEfect ef)
       {
           curBonus = ef;
           curspeed = _moove.Speed;
           _moove.Speed += ef.Speed;
           MaxSpeed += ef.Speed;
           yield return new WaitForSeconds(ef.SpeedTime);
           MaxSpeed -= ef.Speed;
           if (_moove.Speed - ef.Speed >= curspeed)
           {
               _moove.Speed -= ef.Speed;
           }

           curBonus = null;

       }

     
    }
}