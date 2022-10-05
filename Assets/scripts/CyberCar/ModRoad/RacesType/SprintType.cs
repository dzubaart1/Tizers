using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CyberCar.RacesType
{
    public class SprintType: IRoadType
    {
        public Vector3 position;
        private RoadPlaneCntrl prevRoad;
        
        public Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad)
        {
            position = prevRoad.transform.position;
            Vector3 newPosition = new Vector3(-newRoad.Scaler.x/2, 0, position.z+prevRoad.Scaler.z);

            return newPosition;
        }

        public Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad, int prevType, int newType)
        {
          return Vector3.zero;
        }

     

        public void GetTypeInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}