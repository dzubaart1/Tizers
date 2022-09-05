using System;
using CyberCar.Bonuses;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CyberCar.ModCanvas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AlertIconCntrl : MonoBehaviour
    {
        public Image Icon;
        public CanvasGroup _group;
        private bool onshowed;
        private float showTime = 0.7f;
        SignalBus _signalBus;
        public Button _Button;
        private EffectParams MyParams;

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            CanvasGroupCntrl.ChangeStateCanvas(_group,false);
            _Button.onClick.AddListener(SetEfect);
        }

        public void setData(EffectParams _params)
        {
            Icon.sprite = _params.DefenceIcon;
            MyParams = _params;
            CanvasGroupCntrl.ChangeStateCanvas(_group,true);      
            showTime = 2;
            onshowed = true;
        }

        void SetEfect()
        {
            Debug.Log("is clicked");
            SignalSetEfect signal = new SignalSetEfect();
            signal.effect = MyParams;
            _signalBus.Fire(signal);
            HideAlarm();
        }

        private void FixedUpdate()
        {
            if (onshowed && showTime>0)
            {
                showTime -= Time.deltaTime;
            }
            else if(onshowed)
            {
                onshowed = false;
                CanvasGroupCntrl.ChangeStateCanvas(_group,false);
            }
        }

        public void HideAlarm()
        {
            onshowed = false;
            CanvasGroupCntrl.ChangeStateCanvas(_group,false);
        }
    }
}