using System.Collections.Generic;
using UnityEngine;

namespace CyberCar.MenuCntrls
{
    public class CyberCarMenuView: MonoBehaviour
    {
        public List<CyberCarMenuPanel> PanelsList;
        public CyberCarMenuCntrl MenuCntrl = new CyberCarMenuCntrl();
        void Start()
        {
            PanelsList[0].ActivatePanel(true);
        }
        public void StartGame()
        {
            MenuCntrl.RunGame();
        }
        public void ShowPanel(int id)
        {
            foreach (var VARIABLE in PanelsList)
            {
                VARIABLE.ActivatePanel(false);
            }
            PanelsList[id].ActivatePanel(true);
            if (PanelsList[id].isShop)
            {
                PanelsList[id]._view =this;
                PanelsList[id].SetShop();
            }
        }

      

    }
}