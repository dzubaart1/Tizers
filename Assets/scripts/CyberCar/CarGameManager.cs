using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar.ModCanvas;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar
{
    public class CarGameManager : Singleton<CarGameManager>
    {
        //  public Material SkyBox;
        public bool GameStarted;
        public CarCanvasCntrl _CarCanvas;
        public CarCntrl Car;
        public RoadCntrl RoadCntrl;
        public int Score;
        public float NitroBonus;
        public bool Died;
        SignalBus _signalBus;
        private RoadPatern _patern;
        private RoadsParams _roadsParams;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_is_die>(isDie);
            _signalBus.Subscribe<Signal_start_game>(StartGame);
        }

        private IEnumerator Start()
        {
            int PaternId = PlayerPrefs.GetInt("PaternGame");
            Debug.Log(PaternId);
            if (PaternId != 0)
            {
                List<RoadPatern> RoadPaterns = Resources.LoadAll<RoadPatern>("RoadMissions").ToList();
                foreach (var patern in RoadPaterns)
                {
                    if (patern.PaternId == PaternId)
                    {
                        _patern = patern;
                        _roadsParams = _patern.TypeOfRoad;
                        if (!_patern.infinity)
                        {
                            RoadCntrl.Patern = _patern;
                        }

                        RoadCntrl.Params = _patern.TypeOfRoad;
                        RenderSettings.skybox = _roadsParams.SkyBoxMaterial;
                        break;
                    }
                }
            }

           

            /*Car.GameManager = this;
            _CarCanvas.GameManager = this;*/

            yield return new WaitForSeconds(1);
        }

        public void isDie()
        {
            Died = true;
            StartCoroutine(ShowDie());
        } 
        public void isWin()
        {
            Died = true;
            StartCoroutine(ShowWin());
        }

        IEnumerator ShowDie()
        {
            yield return new WaitForSeconds(0.6f);

            _CarCanvas.GameOver();
        }
        IEnumerator ShowWin()
        {
            yield return new WaitForSeconds(0.6f);

            _CarCanvas.GameWin();
        }

        public void StartGame()
        {
            Car.StartGame();
        }

        public void restartScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CyberCar");
            int curscore = PlayerPrefs.GetInt("CoinScore");
            PlayerPrefs.SetInt("CoinScore", curscore + Score);
        }

        public void GoToMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CyberCarMenu");
            int curscore = PlayerPrefs.GetInt("CoinScore");
            PlayerPrefs.SetInt("CoinScore", curscore + Score);
        }

        public void AddScore(int effectScoreCount)
        {
            Score += effectScoreCount;
            _CarCanvas.AddScore();
        }
    }
}