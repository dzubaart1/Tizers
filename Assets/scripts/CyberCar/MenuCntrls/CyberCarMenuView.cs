using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Zenject;

namespace CyberCar.MenuCntrls
{
    public class CyberCarMenuView: MonoBehaviour
    {
        public CyberCarMenuCntrl MenuCntrl = new CyberCarMenuCntrl();
        SignalBus _signalBus;
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
            //_signalBus.Subscribe<Signal_start_game>(StartGame);
          
        }
        void Start()
        {
            ShowPanel(1);
        }
        public void StartGame()
        {
            MenuCntrl.RunGame();
        }
        public void ShowPanel(int id)
        {
            ShowMenuPanle panel = new ShowMenuPanle();
            panel.idPanel = id;
            _signalBus.Fire(panel);
        }

      

    }
}