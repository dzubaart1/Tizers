using System.Collections.Generic;
using UnityEngine;

namespace CyberCar
{
    [CreateAssetMenu(fileName = "CarParams", menuName = "Cars/Params", order = 1)]
    public class CarParams: ScriptableObject
    {
        public string Name;
        public int id;
        public Sprite icon;
        public List<Texture> TexturesList;
        public List<GameObject> BackLights;
        public CarModelCntrl CarModel;
    }
}