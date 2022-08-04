using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace CyberCar.ModCanvas
{
   
    public class CanvasView : MonoBehaviour
    {
        public CarCanvasCntrl _Cntrl;
        public CanvasGroup StartPanel;
        public CanvasGroup GamePanle;
        public CanvasGroup DiePanel;
        public TMP_Text CarSpeed;
        public TMP_Text BackScore;
        public TMP_Text StartTapText;
        public TMP_Text ScoreText;
      

        private void Start()
        {
            BackScore.gameObject.SetActive(false);
            CanvasGroupCntrl.ChangeStateCanvas(StartPanel,true);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle,false);
            CanvasGroupCntrl.ChangeStateCanvas(DiePanel,false);
        }

        public void ShowDiePanel()
        {
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle,false);
            CanvasGroupCntrl.ChangeStateCanvas(DiePanel,true);
        }

        public void SetSpeed(int sp)
        {
            CarSpeed.text = sp*10 + "m/ph";
        }

        public IEnumerator ReadyToStart( )
        {
            StartTapText.gameObject.SetActive(false);
            BackScore.gameObject.SetActive(true);
            int time = 3;
            while (time !=-1)
            {
                yield return new WaitForSeconds(0.5f);
                BackScore.text = time.ToString();
                time -= 1;
            }
            CanvasGroupCntrl.ChangeStateCanvas(StartPanel,false);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle,true);
            _Cntrl.GameManager.StartGame();
            BackScore.gameObject.SetActive(false);
        }

        public void ShowSCore(int gameManagerScore)
        {
            ScoreText.text = gameManagerScore.ToString();
        }
    }
}