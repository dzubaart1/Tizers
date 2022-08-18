using System;
using CyberCar.Bonuses;
using UnityEngine;

namespace Obstacles
{
    public class ObstacleCntrl: MonoBehaviour
    {
        public EffectParams givenEfect;
        public EfectType myType;
       
        public enum EfectType
        {
            none,
            shied,
            prism,
            fire,
            whater
        }

    }
}