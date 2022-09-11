using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TestsScript
{
    public class TestRoadCntrl : Singleton<TestRoadCntrl>
    {
        [Header("Control param")] public RoadPatern Patern;
        private bool IsPatern;
        public RoadsParams Params;
        private int _curRoadPatern;
        private bool prevFront;
        [Header("Game Objects")] private List<RoadPlaneCntrl> RoadFrontList;
        private List<RoadPlaneCntrl> RoadRightList;
        private List<RoadPlaneCntrl> TurnRightList;
        private List<RoadPlaneCntrl> TurnFrontList;
        private List<RoadPlaneCntrl> RoadList = new List<RoadPlaneCntrl>();
        private RoadPlaneCntrl StartRoad;
        private RoadPlaneCntrl prevRoad;
        public GameObject Player;
        public Transform RoadBox;
        public Vector3 position;

        [Header("Updated Params")] [SerializeField]
        private RoadPlaneCntrl prevPlane;

        [SerializeField] private RoadPlaneCntrl curPlane;

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
                curPlane = StartRoad;
                prevRoad = StartRoad;
                for (int i = 0; i < 12; i++)
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
            }
        }

        /*private void FixedUpdate()
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
        }*/

        void CreateNewRoad()
        {
            int rRoad = Random.Range(0, 10);

            if (rRoad > 5)
            {
                position = prevRoad.transform.position;
                if (prevFront)
                {
                    RoadPlaneCntrl road = TurnRightList[Random.Range(0, TurnRightList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x + road.Scaler.x, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 4;
                    prevRoad.SetPlane();
                    prevFront = false;
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    RoadPlaneCntrl road = RoadRightList[Random.Range(0, RoadRightList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x, 0, position.z - road.Scaler.z),
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
                    RoadPlaneCntrl road = TurnFrontList[Random.Range(0, TurnFrontList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x, 0, position.z - road.Scaler.z),
                        Quaternion.identity);
                    prevFront = true;
                    prevRoad.type = 3;
                    prevRoad.SetPlane();
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    RoadPlaneCntrl road = RoadFrontList[Random.Range(0, RoadFrontList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x + road.Scaler.x, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 1;
                    prevFront = true;
                    prevRoad.SetPlane();
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
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

            /*if (RoadList.Count == 5)
            {
                GameObject roadToDel = RoadList[0].gameObject;
                RoadList.Remove(RoadList[0]);
                Destroy(roadToDel);
            }*/
        }

        public void DestroyRoad(RoadPlaneCntrl road)
        {
            if (road != curPlane)
            {
                if (curPlane) prevPlane = curPlane;
                curPlane = road;
                if (prevPlane != null)
                {
                    if (!IsPatern)
                    {
                        CreateNewRoad();
                    }
                    else
                    {
                        CreateNewRoadPatern();
                    }
                    StartCoroutine(DeleteRoad(prevPlane));
                }
            }
        }

        IEnumerator DeleteRoad(RoadPlaneCntrl road)
        {
            yield return new WaitForSeconds(1);
            GameObject roadToDel = road.gameObject;
            RoadList.Remove(road);
            Destroy(roadToDel);
           
        }
    }
}