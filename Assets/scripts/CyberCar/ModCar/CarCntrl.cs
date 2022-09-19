using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar.Bonuses;
using CyberCar.ModShopItems;
using DefaultNamespace;
using Obstacles;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class CarCntrl : Singleton<CarCntrl>
    {
        public float MaxSpeed;
        public float MaxNitroSpeed;
        public CarGameManager GameManager;
        public GameObject ExplodeCar;
        public bool inGame = true;
        public Rigidbody _rb;
        public CarMoove _moove;
        public Animator Anim;
        public CarModelCntrl CarModel;
        private float curspeed;
        private BonusEfect curBonus;
        public float speedBoost;
        public Transform ModelTransform;
        SignalBus _signalBus;
        [Header("Game params")] public GameObject efectObj;
        public List<ObstacleCntrl.EfectType> CurEfects;
        public ObstacleCntrl.EfectType MyEffect;
        public bool effectOnActive;
        public float EfectLifeTime;
        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_turn_car>(TurnCar);
            _signalBus.Subscribe<Signal_nitro>(NitroCar);
            _signalBus.Subscribe<Signal_stop_nitro>(StopNitroCar);
            _signalBus.Subscribe<SignalSetEfect>(ActivateEffect);
            _signalBus.Subscribe<SignalStopEfect>(DeactivateEffect);
            //_signalBus.Fire<PlanetIsReady>();
        }


        void Start()
        {
            Debug.Log(Application.persistentDataPath + "/MySaveData.dat");
            List<CarParams> CarsObjs = Resources.LoadAll<CarParams>("CustomCars").ToList();

            CarSave savedCar = SaveLoadCntrl.LoadGame();
            CarParams GameCar = CarsObjs[1];
            if (savedCar != null)
            {
                for (int i = 0; i < CarsObjs.Count; i++)
                {
                    if (CarsObjs[i].id == savedCar.carParamsId)
                    {
                        GameCar = CarsObjs[i];
                        CarModel = Instantiate(GameCar.CarModel, ModelTransform);
                        if (savedCar.colorId != 0)
                        {
                            List<ColorItem> colorItems = Resources.LoadAll<ColorItem>("ColorItems").ToList();
                            for (int j = 0; j < colorItems.Count; j++)
                            {
                                if (colorItems[j].id == savedCar.colorId)
                                {
                                    CarModel._renderer.material.color = colorItems[j].color;
                                    CarModel._renderer.material.EnableKeyword("_EMISSION");
                                    CarModel._renderer.material.SetVector("_EmissionColor",
                                        colorItems[j].color * (0.1f));
                                }
                            }
                        }

                        if (savedCar.backlightsId != 0)
                        {
                            List<BackLightItem> BackLists =
                                Resources.LoadAll<BackLightItem>("BackLightsItems").ToList();
                            for (int j = 0; j < BackLists.Count; j++)
                            {
                                if (BackLists[j].id == savedCar.backlightsId)
                                    CarModel.setData(BackLists[j].backlight);
                            }
                        }

                        break;
                    }
                }
            }
            else
            {
                CarModel = Instantiate(CarsObjs[1].CarModel, ModelTransform);
            }

            CarModel.transform.rotation = Quaternion.identity;
            ;
            CarModel.transform.localPosition = new Vector3(0, 0.241f, 0);
            // CarModel.setData(CarsObjs[0].BackLights[1]);
            _rb = GetComponent<Rigidbody>();
           // _rb.isKinematic = true;
            _moove = GetComponent<CarMoove>();
            Anim = GetComponent<Animator>();
        }

        void TurnCar()
        {
           // _moove.TurnCar();
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
            _moove.start = true;
            //_rb.isKinematic = false;
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
            if (!GameManager.Died)
            {
                _rb.isKinematic = true;
                _moove.AccelerationSpeed = 0;
                _moove.start = false;
                ExplodeCar.SetActive(true);
                GameManager.isDie();
                Destroy(CarModel.gameObject);
            }
            else
            {
                _moove.Restart();
            }
        }
        public void Finished()
        {
            if (!GameManager.Died)
            {
                _rb.isKinematic = true;
                _moove.AccelerationSpeed = 0;
                _moove.start = false;
              // ExplodeCar.SetActive(true);
                GameManager.isWin();
                //Destroy(CarModel.gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "bonus" && curBonus == null)
            {
                BonusCntrl bonus = other.GetComponent<BonusCntrl>();
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
                    GameManager.NitroBonus += bonus.Effect.NitroCount;
                    GameManager._CarCanvas.UpdateNitroView();
                }
            }

            if (other.tag == "effect")
            {
                ObstacleCntrl effect = other.GetComponent<ObstacleCntrl>();
                _signalBus.Fire(new Signal_Show_alert_icon() {Params = effect.givenEfect});
            }

            if (other.tag == "border")
            {
                _signalBus.Fire<Signal_Hide_alert_icon>();
                if (efectObj == null)
                {
                    DeadEnd();
                    return;
                }

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

            if (other.tag == "finish")
            {
                Finished();
            }
        }


        void ActivateEffect(SignalSetEfect signalParams)
        {
            if (CarModel != null)
            {
                EffectParams effect = signalParams.effect;
                if (efectObj)
                {
                    MyEffect = ObstacleCntrl.EfectType.none;
                    Destroy(efectObj);
                }
                efectObj = Instantiate(effect.EfectObj, CarModel.transform);
                MyEffect = effect.myType;
                EfectLifeTime = effect.EffectTime;
                if (efectObj)
                {
                    efectObj.SetActive(true);
                }
            }
        }
        private void FixedUpdate()
        {
            if (MyEffect != ObstacleCntrl.EfectType.none && EfectLifeTime > 0)
            {
                EfectLifeTime -= Time.deltaTime;
                if (EfectLifeTime <= 0)
                {
                    MyEffect = ObstacleCntrl.EfectType.none;
                    Destroy(efectObj);
                }
            }
           
        }
        private void DeactivateEffect()
        {
            throw new NotImplementedException();
        }

        IEnumerator SpeedBonus(BonusEfect ef)
        {
            curBonus = ef;
            curspeed = _moove.AccelerationSpeed;
            _moove.AccelerationSpeed += ef.Speed;
            MaxSpeed += ef.Speed;
            yield return new WaitForSeconds(ef.SpeedTime);
            MaxSpeed -= ef.Speed;
            if (_moove.AccelerationSpeed - ef.Speed >= curspeed)
            {
                _moove.AccelerationSpeed -= ef.Speed;
            }

            curBonus = null;
        }
    }
}