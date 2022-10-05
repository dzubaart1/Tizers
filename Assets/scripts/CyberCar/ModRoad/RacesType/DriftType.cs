using UnityEngine;

namespace CyberCar.RacesType
{
    public class DriftType: IRoadType
    {
        public Vector3 position;
        private RoadPlaneCntrl prevRoad;
        /// <summary>
        /// 1 - front;
        /// 2 = rigtht
        /// 3 - fturn
        /// 4 = rturn
        /// </summary>
        public Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad)
        {
            position = prevRoad.transform.position;
            Vector3 newPosition = new Vector3(-newRoad.Scaler.x/2, 0, position.z+prevRoad.Scaler.z);

            return newPosition;
        }

        public Vector3 GenerateRoadPosition(RoadPlaneCntrl prevRoad, RoadPlaneCntrl newRoad, int prevType, int newType)
        {
            if (prevType ==1 )
            {
                position = prevRoad.transform.position;
                Vector3 newPosition = new Vector3(position.x, 0, position.z+newRoad.Scaler.z);
                return newPosition;
              
            }

            if (prevType == 2)
            {
                position = prevRoad.transform.position;
                Vector3 newPosition = new Vector3( position.x + prevRoad.Scaler.x , 0, position.z);
                return newPosition; 
            }
            else
            {
                position = prevRoad.transform.position;
                Vector3 newPosition = new Vector3( position.x + prevRoad.Scaler.x , 0, newRoad.Scaler.z+position.z);
                return newPosition;
            }
        }

        

        public void GetTypeInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}