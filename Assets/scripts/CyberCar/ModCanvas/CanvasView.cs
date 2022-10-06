using System;
using System.Collections;
using CyberCar.Bonuses;
using CyberCar.Dictionaries;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.ModCanvas
{
    public class CanvasView : Singleton<CanvasView>
    {
        public CarCanvasCntrl _Cntrl;
        [Header("Panels")] 
        public CanvasGroup StartPanel;
        public CanvasGroup GamePanle;
        public CanvasGroup DiePanel;
        [Header("infos")]
        public TMP_Text CarSpeed;
        public TMP_Text BackScore;
        public TMP_Text StartTapText;
        public TMP_Text ScoreText;
        public Image NitroFill;
        public CanvasGroup NitroEffect;
        public AlertIconCntrl AlertIcon;
        public TMP_Text DieScoreText;
        public TMP_Text DieText;
    
        [Header("Controls")] 
        public CanvasGroup StandartMoveCntrol;

        public ButtonEvents NitroBtn;
        public ButtonEvents BrokeBtn;
        private IEnumerator Start()
        {
            BackScore.gameObject.SetActive(false);
            CanvasGroupCntrl.ChangeStateCanvas(StartPanel, true);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle, false);
            CanvasGroupCntrl.ChangeStateCanvas(DiePanel, false);
            CanvasGroupCntrl.ChangeStateCanvas(NitroEffect, false);
            yield return new WaitForSeconds(3);
            if (_Cntrl.GameManager.RoadCntrl.Trassa == Dictionaryes.TrassaType.drift)
            {
                CanvasGroupCntrl.ChangeStateCanvas(StandartMoveCntrol, false);
            }
            else
            {
                NitroBtn.PointUp.AddListener(delegate { _Cntrl.GameManager.Car._moove.Nitro(_onNitro: false); });
                NitroBtn.PointDown.AddListener(delegate { _Cntrl.GameManager.Car._moove.Nitro(_onNitro: true); });
                BrokeBtn.PointDown.AddListener(delegate { _Cntrl.GameManager.Car._moove.BrakeTorque(inBrake: true); });
                BrokeBtn.PointUp.AddListener(delegate { _Cntrl.GameManager.Car._moove.BrakeTorque(inBrake: false); });
            }
        }

        public void ShowNitroEffect(bool show)
        {
            CanvasGroupCntrl.ChangeStateCanvas(NitroEffect, show);
        }

        public void ShowDiePanel(bool win)
        {
            if (win)
            {
                CanvasGroupCntrl.ChangeStateCanvas(GamePanle, false);
                CanvasGroupCntrl.ChangeStateCanvas(DiePanel, true);
                DieScoreText.text = _Cntrl.GameManager.Score.ToString();
                DieText.text = "Победа";
            }
            else
            {
                CanvasGroupCntrl.ChangeStateCanvas(GamePanle, false);
                CanvasGroupCntrl.ChangeStateCanvas(DiePanel, true);
                DieScoreText.text = _Cntrl.GameManager.Score.ToString();
                DieText.text = "Поражение";
            }
        }

        public void SetSpeed(int sp)
        {
            CarSpeed.text = sp * 10 + "m/ph";
        }

        public IEnumerator ReadyToStart()
        {
            StartTapText.gameObject.SetActive(false);
            BackScore.gameObject.SetActive(true);
            int time = 3;
            while (time != -1)
            {
                yield return new WaitForSeconds(0.5f);
                BackScore.text = time.ToString();
                time -= 1;
            }

            CanvasGroupCntrl.ChangeStateCanvas(StartPanel, false);
            CanvasGroupCntrl.ChangeStateCanvas(GamePanle, true);
            _Cntrl.GameManager.StartGame();
            BackScore.gameObject.SetActive(false);
        }

        public void ShowSCore(int gameManagerScore)
        {
            ScoreText.text = gameManagerScore.ToString();
        }

        public void ShowNitroBalance(float NitroBalance)
        {
            NitroFill.fillAmount = NitroBalance / 100;
        }

        public void AlertOnRoad(EffectParams _effect)
        {
            AlertIcon.setData(_effect);
        }

        public void HideAlertOnRoad()
        {
            AlertIcon.HideAlarm();
        }
    }
}