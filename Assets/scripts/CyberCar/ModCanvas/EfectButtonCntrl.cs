using System;
using System.Collections;
using CyberCar.Bonuses;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

namespace CyberCar.ModCanvas
{
    public class EfectButtonCntrl: MonoBehaviour
    {
        public Image DefenceIcon;
        public Image cdMask;
        private float showTime;
        private CanvasGroup _group;
        private float StartShowTime;
        private bool InShowed;
        private Animator _animator;
        private RectTransform myRect;
        public Button _Button;
        public EffectParams MyParams;
        SignalBus _signalBus;
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(SetEffect);
            StartShowTime = MyParams.showBtnTime;
            cdMask.fillAmount = 0;
        }

        private void SetEffect()
        {
            SignalSetEfect signal = new SignalSetEfect();
            signal.effect = MyParams;
            _signalBus.Fire(signal);
            EffectIsConfirmed();
            InShowed = true;
            cdMask.fillAmount = 1;
            showTime = MyParams.showBtnTime;
        }


        private void FixedUpdate()
        {
            if (InShowed && showTime>0)
            {
                showTime -= Time.deltaTime;
                cdMask.fillAmount = showTime/StartShowTime;
            }
            else
            {
                InShowed = false;
            }
        }

        public void EffectIsConfirmed()
        {
           
            _animator.SetTrigger("show");
        }


    }
}