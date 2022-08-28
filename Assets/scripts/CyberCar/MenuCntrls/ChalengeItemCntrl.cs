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
        public TMP_Text RaceNumber;
        public Image FinishedPanel;
        public Image InfinityMark;


        public void SetData(RoadPatern _patern, ChalengePanelView view)
        {
            View = view;
            //  IconImage.sprite = _patern.Icon;
            RaceNumber.text = _patern.PaternId.ToString();
            if (_patern.isComplited)
            {
                FinishedPanel.gameObject.SetActive(true);
            }
            else
            {
                FinishedPanel.gameObject.SetActive(false);
            }

            if (_patern.infinity)
            {
                InfinityMark.gameObject.SetActive(true);
            }
            else
            {
                InfinityMark.gameObject.SetActive(false);
            }

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