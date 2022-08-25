using System;
using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.ModCanvas
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AlertIconCntrl : MonoBehaviour
    {
        public Image Icon;
        public CanvasGroup _group;
        private bool onshowed;
        private float showTime = 0.7f;

        private void Start()
        {
            _group = GetComponent<CanvasGroup>();
            CanvasGroupCntrl.ChangeStateCanvas(_group,false);
        }

        public void setData(Sprite _icon)
        {
            Icon.sprite = _icon;
            CanvasGroupCntrl.ChangeStateCanvas(_group,true);      
            showTime = 2;
            onshowed = true;
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