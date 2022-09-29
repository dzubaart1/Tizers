using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CyberCar
{
    public class RoadCollection: MonoBehaviour
    {
        public RoadPatern Patern;
        public RoadsParams Params;
        private bool IsPatern;
        public List<RoadPlaneCntrl> RoadFrontList;
        public List<RoadPlaneCntrl> RoadRightList;
        public List<RoadPlaneCntrl> TurnRightList;
        public List<RoadPlaneCntrl> TurnFrontList;
        
        public  bool prevFront;
       public  RoadCollection(RoadPatern _patern = null, RoadsParams _params = null)
        {
            Patern = _patern;
            Params = _params;

        }

        public void CollectRoads()
        {
             if (Patern == null)
            {
                IsPatern = false;
                if (Params != null)
                {
                    List<RoadPlaneCntrl> RoadList = Resources.LoadAll<RoadPlaneCntrl>(Params.PathToRoads).ToList();
                    RoadFrontList = new List<RoadPlaneCntrl>();
                    RoadRightList = new List<RoadPlaneCntrl>();
                    TurnFrontList = new List<RoadPlaneCntrl>();
                    TurnRightList = new List<RoadPlaneCntrl>();
                    foreach (RoadPlaneCntrl road in RoadList)
                    {
                        if (!road.IsFinish)
                        {
                            if (road.type == 1) RoadFrontList.Add(road);
                            if (road.type == 2) RoadRightList.Add(road);
                            if (road.type == 3) TurnFrontList.Add(road);
                            if (road.type == 4) TurnRightList.Add(road);
                        }
                    }
                }
                else
                {
                    RoadFrontList = Resources.LoadAll<RoadPlaneCntrl>("front").ToList();
                    RoadRightList = Resources.LoadAll<RoadPlaneCntrl>("right").ToList();
                    TurnFrontList = Resources.LoadAll<RoadPlaneCntrl>("turnF").ToList();
                    TurnRightList = Resources.LoadAll<RoadPlaneCntrl>("turnR").ToList();
                }

                prevFront = true;
               
            }
            /*else
            {
                IsPatern = true;
                prevFront = true;
                StartRoad = Instantiate(Patern.Roads[0], new Vector3(0, 0, 0), Quaternion.identity);
                _curRoadPatern = 1;
                prevRoad = StartRoad;
            }*/
        }
    }
}