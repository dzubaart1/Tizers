using UnityEngine;
using UnityEngine.UI;

namespace CyberCar.MenuCntrls
{
    public class ColorBtn : MonoBehaviour
    {
        public Image MyImage;
        public Button MyButton;
        public Color myColor;
        public ShopView View;
        public int MyColorID;

        public void SetData(Color color, ShopView _view, int colorId)
        {
            MyColorID = colorId;
            MyImage = GetComponent<Image>();
            myColor = color;
            MyImage.color = color;
            View = _view;
            MyButton = GetComponent<Button>();
            MyButton.onClick.AddListener(SetColor);
        }

        private void SetColor()
        {
            View.setColorCar(myColor,MyColorID);

        }
    }
}