using UnityEngine;

namespace CyberCar
{
    [CreateAssetMenu(fileName = "RoadParams", menuName = "Roads/RoadsParams", order = 1)]
    public class RoadsParams : ScriptableObject
    {
        public int ParamsId;
        public string PathToRoads;
        public Material SkyBoxMaterial;

    }
}