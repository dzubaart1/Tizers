using System;
using CyberCar.Bonuses;
using GameItems;
using UnityEngine;

namespace Obstacles
{
    public class ObstacleCntrl: MonoBehaviour, IGameItem
    {
        public EffectParams givenEfect;
        public IGameItem.ItemType ItemType;
        public IGameItem.EfectType myType;

        public void Destroyitem()
        {
            return;
        }

        public void SetEfectItem()
        {
            return;
        }

        public void InteractItem()
        {
            return;
        }
    }
}