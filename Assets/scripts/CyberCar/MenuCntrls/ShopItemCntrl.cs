using CyberCar.ModShopItems;
using TMPro;
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
        public TMP_Text Price;
        private ColorItem _colorItem;
        private BackLightItem _backLight;
        public void SetData(CarParams car,ShopView view)
        {
            View = view;
            myCar = car;
            IconImage.sprite = car.icon;
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(SetMyCar);
            Price.text = car.price.ToString();
        }  
        public void SetData(ColorItem color,ShopView view)
        {
            View = view;
            _colorItem = color;
            IconImage.color = color.color;
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(SetColor);
            Price.text = color.price.ToString();
        } 
        public void SetData(BackLightItem backLight,ShopView view)
        {
            View = view;
            _backLight = backLight;
            IconImage.color = backLight.color;
            _Button = GetComponent<Button>();
            _Button.onClick.AddListener(SetBackLight);
            Price.text = backLight.price.ToString();
        }

        void SetMyCar()
        {
            View.setShopCar(myCar);
        }
        void SetColor()
        {
            View.setColorCar(_colorItem.color, _colorItem.id);
        }
        void SetBackLight()
        {
            View.setBacklightCar(_backLight, _backLight.id);
        }
    }
}