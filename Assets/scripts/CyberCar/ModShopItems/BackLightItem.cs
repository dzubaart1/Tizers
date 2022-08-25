using UnityEngine;

namespace CyberCar.ModShopItems
{
    [CreateAssetMenu(fileName = "BacklightItem", menuName = "ShopItems/BackLight", order = 1)]
    public class BackLightItem: ScriptableObject
    {
        public int id;
        public GameObject backlight;
        public int price;
        public bool IsBuyed;
        public Color color;
    }
}