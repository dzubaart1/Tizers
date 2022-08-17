using DefaultNamespace;
using Zenject;

namespace CyberCar.ModCanvas
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
 
    public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler {
        [ Tooltip( "How long must pointer be down on this object to trigger a long press" ) ]
        public float durationThreshold = 1.0f;
 
        public UnityEvent onLongPress = new UnityEvent( );
        public UnityEvent onShortPress = new UnityEvent( );
        public UnityEvent stopDataPress = new UnityEvent( );
 
        private bool isPointerDown = false;
        private bool longPressTriggered = false;
        private float timePressStarted;
        private bool Showed;
        SignalBus _signalBus;
        
        [Inject]
        public void Construct( SignalBus signalBus)
        {
            _signalBus = signalBus;
            
            
        }
        private void Update( ) {
            if ( isPointerDown && !longPressTriggered ) {
                if ( Time.time - timePressStarted > durationThreshold ) {
                    longPressTriggered = true;
                    if (!Showed)
                    {
                        _signalBus.Fire<Signal_nitro>();
                        Showed = true;
                    }
                }
            }
        }
 
        public void OnPointerDown( PointerEventData eventData ) {
            timePressStarted = Time.time;
            isPointerDown = true;
            longPressTriggered = false;
        }
 
        public void OnPointerUp( PointerEventData eventData ) {
            if (Time.time - timePressStarted < durationThreshold)
            {
                _signalBus.Fire<Signal_turn_car>();
               // onShortPress.Invoke( );
            }
            _signalBus.Fire<Signal_stop_nitro>();
            Showed = false;
          //  stopDataPress.Invoke( );
            isPointerDown = false;
        }
 
 
        public void OnPointerExit( PointerEventData eventData ) {
            isPointerDown = false;
        }
    }
}