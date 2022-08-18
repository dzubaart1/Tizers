using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
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

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _group = gameObject.AddComponent<CanvasGroup>();
            CanvasGroupCntrl.ChangeStateCanvas(_group,false);
            myRect = GetComponent<RectTransform>();
        }

        public void SetData(Sprite defenceSprite, float _showtime)
        {
            DefenceIcon.sprite = defenceSprite;
            cdMask.fillAmount = 1;
            StartShowTime = _showtime;
            showTime = _showtime;
            InShowed = true;
            myRect.anchoredPosition  = new Vector2(Random.Range(-300,300), Random.Range(-100, 100));
            CanvasGroupCntrl.ChangeStateCanvas(_group,true);
            _animator.SetTrigger("instant");
          
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
                CanvasGroupCntrl.ChangeStateCanvas(_group,false);
                InShowed = false;
            }
        }

        public void EffectIsConfirmed()
        {
           
            _animator.SetTrigger("show");
        }

        public void HideButton()
        {
            CanvasGroupCntrl.ChangeStateCanvas(_group,false); 
            showTime = 0;
            cdMask.fillAmount = 1;
            StartShowTime = 0;
        }

    }
}