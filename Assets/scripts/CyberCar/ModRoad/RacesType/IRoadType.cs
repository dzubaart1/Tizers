using UnityEngine;

namespace CyberCar.RacesType
{
    public interface IRoadType
    {
        Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad);
        void GetTypeInfo();
        
    }
}