using UnityEngine;

namespace CyberCar.ModShopItems
{
    [CreateAssetMenu(fileName = "ColorItem", menuName = "ShopItems/Color", order = 1)]
    public class ColorItem : ScriptableObject
    {
        public int id;
        public Color color;
        public int price;
        public bool IsBuyed;
    }
}