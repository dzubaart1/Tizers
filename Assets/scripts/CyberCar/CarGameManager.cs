using System;
using System.Collections;
using CyberCar.ModCanvas;
using UnityEngine;

namespace CyberCar
{
    public class CarGameManager: MonoBehaviour
    {
        public bool GameStarted;
        public CarCanvasCntrl _CarCanvas;
        public CarCntrl Car;
        public RoadCntrl RoadCntrl;
        public int Score;
        public float NitroBonus;

        private IEnumerator  Start()
        {
            Car.GameManager = this;
            _CarCanvas.GameManager = this;
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
        }
        public void GoToMenuScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("CyberCarMenu");
        }

        public void AddScore(int effectScoreCount)
        {
            Score += effectScoreCount;
            _CarCanvas.AddScore();
        }
    }
}