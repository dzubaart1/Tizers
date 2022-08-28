using System.Collections.Generic;
using UnityEngine;

namespace CyberCar
{
    [CreateAssetMenu(fileName = "RoadParams", menuName = "Roads/RoadPatern", order = 1)]
    public class RoadPatern : ScriptableObject
    {
        public int PaternId;
        public string Name;
        public Sprite Icon;
        public bool infinity;
        public List<RoadPlaneCntrl> Roads;
        public RoadPlaneCntrl FinishPlane;
        public RoadsParams TypeOfRoad;
        public bool isComplited;
    }
}