using System;
using System.Collections.Generic;
using CyberCar.ModShopItems;
using UnityEngine;

namespace CyberCar
{
    [CreateAssetMenu(fileName = "CarParams", menuName = "Cars/Params", order = 1)]
    public class CarParams: ScriptableObject
    {
        public int id;
        public int price;
        public string Name;
        public Sprite icon;
        public List<Color> TexturesColor;
        public List<ColorItem> AvalibleColors;
        public bool IsBuyed;
     //   public List<Texture> TexturesList;
        //public List<GameObject> BackLights;
        public CarModelCntrl CarModel;
    }
}