using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class RoadCntrl : MonoBehaviour
    {
        private bool prevFront ;
        public List<RoadPlaneCntrl> RoadFrontList;
        public List<RoadPlaneCntrl> RoadRightList;
        public List<RoadPlaneCntrl> TurnRightList;
        public List<RoadPlaneCntrl> TurnFrontList;
        public RoadPlaneCntrl StartRoad;
        private RoadPlaneCntrl prevRoad;
        public CarCntrl Player;
        public List<RoadPlaneCntrl> RoadList;
        public Transform RoadBox;

        void Start()
        {
            RoadFrontList = Resources.LoadAll<RoadPlaneCntrl>("front").ToList();
            RoadRightList = Resources.LoadAll<RoadPlaneCntrl>("right").ToList();
            TurnFrontList = Resources.LoadAll<RoadPlaneCntrl>("turnF").ToList();
            TurnRightList = Resources.LoadAll<RoadPlaneCntrl>("turnR").ToList();
            prevFront = true;
            prevRoad = StartRoad;
            for (int i = 0; i < 5; i++)
            {
                CreateNewRoad();
            }

        }

        private void FixedUpdate()
        {
            if (Vector3.Distance(Player.gameObject.transform.position, prevRoad.gameObject.transform.position) < 50)
            {
                CreateNewRoad();
            }
        }

        void CreateNewRoad()
        {
            int rRoad = Random.Range(0, 10);

            if (rRoad > 5)
            {
                var position = prevRoad.transform.position;
                if (prevFront)
                {
                    
                    prevRoad = Instantiate(TurnRightList[Random.Range(0,TurnRightList.Count-1)],
                        new Vector3(position.x + 30, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 4;
                    prevRoad.SetPlane(new Vector3(position.x + 30, 0, position.z));
                    prevFront = false;
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    prevRoad = Instantiate(RoadRightList[Random.Range(0,RoadRightList.Count-1)],
                        new Vector3(position.x, 0, position.z - 30),
                        Quaternion.identity);
                    prevRoad.type = 2;
                    prevRoad.SetPlane(new Vector3(position.x, 0, position.z - 30));
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                
               
            }
            else
            {
                var position = prevRoad.transform.position;
                if (!prevFront)
                {
                    prevRoad = Instantiate(TurnFrontList[Random.Range(0,TurnFrontList.Count-1)],
                        new Vector3(position.x, 0, position.z - 30),
                        Quaternion.identity);
                    prevFront = true;
                    prevRoad.type = 3;
                    prevRoad.SetPlane(new Vector3(position.x, 0, position.z - 30));
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    prevRoad = Instantiate(RoadFrontList[Random.Range(0,RoadFrontList.Count-1)],
                        new Vector3(position.x + 30, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 1;
                    prevFront = true;
                    prevRoad.SetPlane(new Vector3(position.x + 30, 0, position.z));
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
               
            }

            if (RoadList.Count == 15)
            {
                GameObject roadToDel = RoadList[0].gameObject;
                RoadList.Remove(RoadList[0]);
                Destroy(roadToDel);
               
            }
        }
    }
}