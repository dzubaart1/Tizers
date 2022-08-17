using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar.Bonuses;
using DefaultNamespace;
using Obstacles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class CarCntrl: Singleton<CarCntrl>
    {
        public float MaxSpeed;
        public float MaxNitroSpeed;
        public CarGameManager GameManager;
        public GameObject ExplodeCar;
        public bool inGame =true;
        public Rigidbody _rb;
        public  CarMoove _moove;
        public Animator Anim;
        public CarModelCntrl CarModel;
        private float curspeed;
        private BonusEfect curBonus;
        public float speedBoost;
        SignalBus _signalBus;
        [Header("Game params")]
        public GameObject efectObj;
        public ObstacleCntrl.EfectType MyEffect;
        public bool effectOnActive;
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_turn_car>(TurnCar);
            _signalBus.Subscribe<Signal_nitro>(NitroCar);
            _signalBus.Subscribe<Signal_stop_nitro>(StopNitroCar);
            _signalBus.Subscribe<Signal_Show_Get_Effect>(ActivateEffect);
         //_signalBus.Fire<PlanetIsReady>();
        }
        void Start()
        {
            List<CarParams> CarsObjs = Resources.LoadAll<CarParams>("CustomCars").ToList();
            CarModel = Instantiate(CarsObjs[1].CarModel,transform);
            CarModel.transform.localPosition = new Vector3(0, 0.241f, 0);
            CarModel.setData(CarsObjs[0].BackLights[1], CarsObjs[0].TexturesList[2]);
            _rb = GetComponent<Rigidbody>();
            _rb.isKinematic = true;
            _moove = GetComponent<CarMoove>();
            Anim = GetComponent<Animator>();
        }
        void TurnCar()
        {
            _moove.TurnCar();
        }

        void NitroCar()
        {
            Debug.Log("Nitro called");
            _moove._onNitro = true;
        }
        void StopNitroCar()
        {
            _moove._onNitro = false;
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
        public void DeadEnd()
        {
            _rb.isKinematic = true;
            ExplodeCar.SetActive(true);
            GameManager.isDie();
            
            
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
            if (other.tag == "effect")
            {
                efectObj = Instantiate(new GameObject() ,CarModel.transform);
                MeshFilter filter = efectObj.AddComponent<MeshFilter>();
                filter.mesh = other.GetComponent<ObstacleCntrl>().givenEfect.mesh;
                MeshRenderer rend = efectObj.AddComponent<MeshRenderer>();
                rend.material = other.GetComponent<ObstacleCntrl>().givenEfect.Material;
                MyEffect = other.GetComponent<ObstacleCntrl>().myType;
                Destroy(other.GetComponent<BoxCollider>());
                efectObj.SetActive(false);
                _signalBus.Fire<Signal_Show_effect_button>();
            }
            if (other.tag == "border")
            {
                if (other.GetComponent<ObstainsBlockCntrl>().neededEffect == MyEffect && efectObj.activeSelf)
                {
                    Destroy(other.gameObject);
                    Destroy(efectObj);
                }
                else
                {
                    DeadEnd();
                }
            }
        }


        void ActivateEffect()
        {
            if (efectObj)
            {
                efectObj.SetActive(true);
            }
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