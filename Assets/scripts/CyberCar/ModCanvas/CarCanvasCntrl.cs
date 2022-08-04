using System;
using UnityEngine;

namespace CyberCar.ModCanvas
{
    [RequireComponent(typeof(CanvasView))]
    public class CarCanvasCntrl: MonoBehaviour
    {
        public CarGameManager GameManager;
        public CanvasView _view;
        private bool readytoStart;
      
        public void StartUiGame()
        {
            if (readytoStart)  return ;
            readytoStart = true;
            _view = GetComponent<CanvasView>();
            _view._Cntrl = this;
            StartCoroutine(_view.ReadyToStart());
        }

        public void GameOver()
        {
            _view.ShowDiePanel();
        }

        public void AddScore()
        {
            _view.ShowSCore(GameManager.Score);
        }
    }
}