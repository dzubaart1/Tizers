using System;
using System.Collections;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.ModCanvas
{
   
    public class CanvasView : Singleton<CanvasView>
    {
        public CarCanvasCntrl _Cntrl;
        public CanvasGroup StartPanel;
        public CanvasGroup GamePanle;
        public CanvasGroup DiePanel;
        public TMP_Text CarSpeed;
        public TMP_Text BackScore;
        public TMP_Text StartTapText;
        public TMP_Text ScoreText;
        public Image NitroFill;
        public CanvasGroup NitroEffect;
        public EfectButtonCntrl EffectButton;

        private void Start()
        {
            BackScore.gameObject.SetActive(false);
            CanvasGroupCntrl.ChangeStateCanvas(StartPanel,true);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle,false);
            CanvasGroupCntrl.ChangeStateCanvas(DiePanel,false);
            CanvasGroupCntrl.ChangeStateCanvas(NitroEffect,false);
        }

        public void ShowNitroEffect(bool show)
        {
            CanvasGroupCntrl.ChangeStateCanvas(NitroEffect,show); 
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
        public void ShowNitroBalance(float NitroBalance)
        {
            NitroFill.fillAmount = NitroBalance/100;
        }

        public void ShowEffectBtn(Signal_Show_effect_button effectButton)
        {
            EffectButton.SetData(effectButton.effecticon,effectButton.showTime);
        }
    }
}