using System;
using System.Collections;
using CyberCar.ModCanvas;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar
{
    public class CarGameManager: Singleton<CarGameManager>
    {
        public bool GameStarted;
        public CarCanvasCntrl _CarCanvas;
        public CarCntrl Car;
        public RoadCntrl RoadCntrl;
        public int Score;
        public float NitroBonus;
        SignalBus _signalBus;
        
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_is_die>(isDie);
            _signalBus.Subscribe<Signal_start_game>(StartGame);
          
        }
        private IEnumerator  Start()
        {
            /*Car.GameManager = this;
            _CarCanvas.GameManager = this;*/
            yield return new WaitForSeconds(1);
            
        }

        public void isDie()
        {
            StartCoroutine(ShowDie());
        }

        IEnumerator ShowDie()
        {
            yield return new WaitForSeconds(0.6f);
           
            _CarCanvas.GameOver();
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