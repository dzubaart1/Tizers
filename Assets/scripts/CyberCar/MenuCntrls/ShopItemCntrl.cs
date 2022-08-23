using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.MenuCntrls
{
    public class ShopItemCntrl: MonoBehaviour
    {
        public Image IconImage;
        public Button _Button;
        public CarParams myCar;
        public ShopView View;
        public void SetData(CarParams car,ShopView view)
        {
            View = view;
            myCar = car;
            IconImage.sprite = car.icon;
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(SetMyCar);
        }

        void SetMyCar()
        {
            View.setShopCar(myCar);
        }
    }
}