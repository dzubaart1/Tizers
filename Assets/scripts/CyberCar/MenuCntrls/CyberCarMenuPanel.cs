using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace CyberCar.MenuCntrls
{
    public class CyberCarMenuPanel : MonoBehaviour
    {
        public int PanelId;
        public CanvasGroup _CanvasGroup;
        public bool isShop;
        public ShopItemCntrl ShopItem;
        public Transform ShopContainer;
        public CyberCarMenuView _view;
        public UnityEvent ShowPanel;
        public UnityEvent HidePanel;
        SignalBus _signalBus;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<ShowMenuPanle>(CheckOnShow);
           
        }

        void CheckOnShow(ShowMenuPanle panel)
        {
            CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup, panel.idPanel == PanelId);
        }


        private void Start()
        {
            _CanvasGroup = GetComponent<CanvasGroup>();
             CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup,false);
        }
    }
}