using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberCar.MenuCntrls
{
    public class CyberCarMenuPanel: MonoBehaviour
    {
        public CanvasGroup _CanvasGroup;
        public bool isShop;
        public ShopItemCntrl ShopItem;
        public Transform ShopContainer;
        public CyberCarMenuView _view;

        private void Start()
        {
            _CanvasGroup = GetComponent<CanvasGroup>();
            CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup,false);
        }

        public void ActivatePanel(bool active)
        {
            CanvasGroupCntrl.ChangeStateCanvas(_CanvasGroup,active);
        }

        public void SetShop()
        {
            foreach(Transform child in ShopContainer) {
                Destroy(child.gameObject);
            }
            List<CarParams> Cars = _view.MenuCntrl.GetCarsList();
            foreach (var car in Cars)
            {
                ShopItemCntrl item = Instantiate(ShopItem, ShopContainer);
                item.SetData(car.icon);
            }
        }
    }
}