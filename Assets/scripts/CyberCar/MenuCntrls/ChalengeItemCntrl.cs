using CyberCar.ModShopItems;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace CyberCar.MenuCntrls
{
    public class ChalengeItemCntrl : MonoBehaviour
    {
        public Image IconImage;
        public Button _Button;
        public RoadPatern Patern;
        public ChalengePanelView View;
        
        public void SetData(RoadPatern _patern,ChalengePanelView view)
        {
            View = view;
            IconImage.sprite = _patern.Icon;
            _Button = GetComponent<Button>();
            Patern = _patern;
              _Button.onClick.AddListener(setPaternGame);
        }

        void setPaternGame()
        {
            View.SetPaternId(Patern.PaternId);
           
        }
    }
}