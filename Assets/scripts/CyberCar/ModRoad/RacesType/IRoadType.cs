using UnityEngine;

namespace CyberCar.RacesType
{
    public interface IRoadType
    {
        Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad);
        Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad,int prevType,int newType);
        void GetTypeInfo();
        
    }
}