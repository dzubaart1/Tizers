using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.MenuCntrls
{
    public class ShopItemCntrl: MonoBehaviour
    {
        public Image IconImage;

        public void SetData(Sprite iconSprite)
        {
            IconImage.sprite = iconSprite;
        }
    }
}