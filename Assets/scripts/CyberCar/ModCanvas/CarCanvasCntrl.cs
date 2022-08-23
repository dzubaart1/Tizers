using System;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar.ModCanvas
{
    [RequireComponent(typeof(CanvasView))]
    public class CarCanvasCntrl: MonoBehaviour
    {
        public CarGameManager GameManager;
        public CanvasView _view;
        private bool readytoStart;
        SignalBus _signalBus;
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<Signal_nitro>(ShowNitro);
            _signalBus.Subscribe<Signal_stop_nitro>(HideNitro);
            _signalBus.Subscribe<Signal_Show_alert_icon>(ShowAlertIcon);
          
        }

        private void ShowAlertIcon(Signal_Show_alert_icon signal)
        {
            _view.AlertOnRoad(signal.effecticon);
        }

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
        public void UpdateNitroView()
        {
            _view.ShowNitroBalance(GameManager.NitroBonus);
        }

        void ShowNitro()
        {
            ShowNitroEffect(true);
        }

        void HideNitro()
        {
            ShowNitroEffect(false);
        }

        void ShowNitroEffect(bool show)
        {
            _view.ShowNitroEffect(show);
        }

        public void RestartGame()
        {
            GameManager.restartScene();
        }
        public void BackToMenu()
        {
            GameManager.GoToMenuScene();
        }

        
    }
}