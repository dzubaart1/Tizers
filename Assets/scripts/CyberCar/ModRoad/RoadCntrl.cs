using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class RoadCntrl : MonoBehaviour
    {
        private bool prevFront;
        public List<RoadPlaneCntrl> RoadFrontList;
        public List<RoadPlaneCntrl> RoadRightList;
        public List<RoadPlaneCntrl> TurnRightList;
        public List<RoadPlaneCntrl> TurnFrontList;
        public RoadPlaneCntrl StartRoad;
        private RoadPlaneCntrl prevRoad;
        public CarCntrl Player;
        public List<RoadPlaneCntrl> RoadList;
        public Transform RoadBox;
        public RoadPatern Patern;
        private bool IsPatern;
        public RoadsParams Params;
        private int _curRoadPatern;
        public Vector3 position;

        void Start()
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
                StartRoad = Instantiate(RoadFrontList[0], new Vector3(0, 0, 0), Quaternion.identity);
                prevRoad = StartRoad;
                Player = CarCntrl.Instance;
                for (int i = 0; i < 2; i++)
                {
                    CreateNewRoad();
                }
            }
            else
            {
                IsPatern = true;
                prevFront = true;
                StartRoad = Instantiate(Patern.Roads[0], new Vector3(0, 0, 0), Quaternion.identity);
                _curRoadPatern = 1;
                prevRoad = StartRoad;
                Player = CarCntrl.Instance;
            }
        }

        private void FixedUpdate()
        {
            if (!IsPatern)
            {
                if (Vector3.Distance(Player.gameObject.transform.position, prevRoad.gameObject.transform.position) < 50)
                {
                    CreateNewRoad();
                }
            }
            else
            {
                if (Vector3.Distance(Player.gameObject.transform.position, prevRoad.gameObject.transform.position) < 50)
                {
                    Debug.Log("enter to patern create");
                    CreateNewRoadPatern();
                }
            }
        }

        void CreateNewRoad()
        {
            int rRoad = Random.Range(0, 10);

            if (rRoad > 5)
            {
                position = prevRoad.transform.position;
                if (prevFront)
                {
                    prevRoad = Instantiate(TurnRightList[Random.Range(0, TurnRightList.Count - 1)],
                        new Vector3(position.x + 30, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 4;
                    prevRoad.SetPlane();
                    prevFront = false;
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    prevRoad = Instantiate(RoadRightList[Random.Range(0, RoadRightList.Count - 1)],
                        new Vector3(position.x, 0, position.z - 30),
                        Quaternion.identity);
                    prevRoad.type = 2;
                    prevRoad.SetPlane();
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
            }
            else
            {
                var position = prevRoad.transform.position;
                if (!prevFront)
                {
                    prevRoad = Instantiate(TurnFrontList[Random.Range(0, TurnFrontList.Count - 1)],
                        new Vector3(position.x, 0, position.z - 30),
                        Quaternion.identity);
                    prevFront = true;
                    prevRoad.type = 3;
                    prevRoad.SetPlane();
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    prevRoad = Instantiate(RoadFrontList[Random.Range(0, RoadFrontList.Count - 1)],
                        new Vector3(position.x + 30, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 1;
                    prevFront = true;
                    prevRoad.SetPlane();
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

        void CreateNewRoadPatern()
        {
            _curRoadPatern++;
            if (Patern.Roads.Count > _curRoadPatern)
            {
                position = prevRoad.transform.position;
                switch (Patern.Roads[_curRoadPatern].type)
                {
                    case 1:
                        prevRoad = Instantiate(Patern.Roads[_curRoadPatern],
                            new Vector3(position.x + 30, 0, position.z),
                            Quaternion.identity);
                        break;
                    case 2:
                        prevRoad = Instantiate(Patern.Roads[_curRoadPatern],
                            new Vector3(position.x, 0, position.z - 30),
                            Quaternion.identity);

                        break;
                    case 3:
                        prevRoad = Instantiate(Patern.Roads[_curRoadPatern],
                            new Vector3(position.x, 0, position.z - 30),
                            Quaternion.identity);

                        break;
                    case 4:
                        prevRoad = Instantiate(Patern.Roads[_curRoadPatern],
                            new Vector3(position.x + 30, 0, position.z),
                            Quaternion.identity);
                        break;
                }

                prevFront = true;
                if (prevRoad is { })
                {
                    prevRoad.SetPlane();
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
            }

            if (_curRoadPatern == Patern.Roads.Count)
            {
                position = prevRoad.transform.position;
                switch (Patern.FinishPlane.type)
                {
                    case 1:
                        prevRoad = Instantiate(Patern.FinishPlane,
                            new Vector3(position.x + 30, 0, position.z),
                            Quaternion.identity);
                        break;
                    case 2:
                        prevRoad = Instantiate(Patern.FinishPlane,
                            new Vector3(position.x, 0, position.z - 30),
                            Quaternion.identity);

                        break;
                    case 3:
                        prevRoad = Instantiate(Patern.FinishPlane,
                            new Vector3(position.x, 0, position.z - 30),
                            Quaternion.identity);

                        break;
                    case 4:
                        prevRoad = Instantiate(Patern.FinishPlane,
                            new Vector3(position.x + 30, 0, position.z),
                            Quaternion.identity);
                        break;
                }

                prevFront = true;
                if (prevRoad is { })
                {
                    prevRoad.SetPlane();
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