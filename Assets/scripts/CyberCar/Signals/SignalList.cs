using CyberCar.Bonuses;
using UnityEngine;

namespace DefaultNamespace
{
    public class SignalList
    {
    }

//Die of player
    public class Signal_is_die
    {
    }
    //Start the game
    public class Signal_start_game
    {
    }
    //Start the game
    public class Signal_turn_car { }
    public class Signal_nitro { }
    public class Signal_stop_nitro { }
    public class Signal_Show_alert_icon {
        public Sprite effecticon = null;
        public EffectParams Params;
    } 
    public class Signal_Hide_alert_icon {
       
    }

    public class SignalSetEfect
    {
        public EffectParams effect;
    }

    public class SignalStopEfect
    {
        public EffectParams effect;  
    }

    
}