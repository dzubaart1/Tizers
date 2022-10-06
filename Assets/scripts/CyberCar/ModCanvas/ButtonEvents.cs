
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace CyberCar.ModCanvas
{
    public class ButtonEvents: MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
         public UnityEvent PointDown;
         public UnityEvent PointUp;
        [SerializeField] UnityEvent PointClick;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if(PointDown!=null) PointDown.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if(PointUp!=null) PointUp.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if(PointClick!=null) PointClick.Invoke();
        }

       
    }
}