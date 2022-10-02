using UnityEngine;

namespace GameItems
{
    public interface IGameItem
    {
        public enum ItemType
        {
            obstacle,
            bonus
        }
        public enum EfectType
        {
            none,
            shied,
            prism,
            fire,
            whater
        }

        void Destroyitem();
        void SetEfectItem();
        void InteractItem();
        
    }
}