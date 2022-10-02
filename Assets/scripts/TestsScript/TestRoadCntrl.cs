using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CyberCar;
using CyberCar.RacesType;
using UnityEngine;

namespace TestsScript
{
    public class TestRoadCntrl : Singleton<TestRoadCntrl>
    {
        [Header("Control param")]
        public RoadPatern Patern;
        private bool IsPatern;
        public RoadsParams Params;
        private int _curRoadPatern;
        private bool prevFront;
        [Header("Types Roads")] 
        public IRoadType RoadType;

        [Header("Game Objects")] 
      [SerializeField]  public RoadCollection _roadCollection;
     
        private List<RoadPlaneCntrl> RoadList = new List<RoadPlaneCntrl>();
        private RoadPlaneCntrl StartRoad;
        private RoadPlaneCntrl prevRoad;
        public GameObject Player;
        public Transform RoadBox;
        public Vector3 position;

        [Header("Updated Params")] [SerializeField]
        private RoadPlaneCntrl prevPlane;

        [SerializeField] private RoadPlaneCntrl curPlane;
        public bool onlyFront;
        public bool withoutBonus;
        void Start()
        {
            RoadType = new SprintType();
            _roadCollection = gameObject.AddComponent<RoadCollection>();
            _roadCollection.Params = Params;
            _roadCollection.Patern = Patern;
            _roadCollection.CollectRoads();
            CreateStartWorld();
        }

        void CreateStartWorld()
        {
            StartRoad = Instantiate(_roadCollection.RoadFrontList[0], new Vector3(-_roadCollection.RoadFrontList[0].Scaler.x/2, 0, -_roadCollection.RoadFrontList[0].Scaler.z/2), Quaternion.identity);
            curPlane = StartRoad;
            prevRoad = StartRoad;
            prevRoad.transform.parent = RoadBox;
            for (int i = 0; i < 12; i++)
            {
                CreateNewRoad();
            }
        }


        void CreateNewRoad()
        {
            int rRoad = Random.Range(0, 10);
            if (onlyFront)
            { 
                RoadPlaneCntrl road = _roadCollection.RoadFrontList[Random.Range(0, _roadCollection.RoadFrontList.Count )];
                Vector3 newPos = RoadType.GenerateRoadPosition(prevRoad, road);
                position = prevRoad.transform.position;
                prevRoad = Instantiate(road, newPos, Quaternion.identity);
                prevRoad.type = 1;
                prevRoad.SetPlane();
                RoadList.Add(prevRoad);
                prevRoad.transform.parent = RoadBox;
                return;
            }

            if (rRoad > 5)
            {
                position = prevRoad.transform.position;
                if (prevFront)
                {
                    RoadPlaneCntrl road = _roadCollection.TurnRightList[Random.Range(0, _roadCollection.TurnRightList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x + road.Scaler.x, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 4;
                    prevRoad.SetPlane(withoutBonus);
                    prevFront = false;
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    RoadPlaneCntrl road = _roadCollection.RoadRightList[Random.Range(0, _roadCollection.RoadRightList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x, 0, position.z - road.Scaler.z),
                        Quaternion.identity);
                    prevRoad.type = 2;
                    prevRoad.SetPlane(withoutBonus);
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
            }
            else
            {
                var position = prevRoad.transform.position;
                if (!prevFront)
                {
                    RoadPlaneCntrl road = _roadCollection.TurnFrontList[Random.Range(0, _roadCollection.TurnFrontList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x, 0, position.z - road.Scaler.z),
                        Quaternion.identity);
                    prevFront = true;
                    prevRoad.type = 3;
                    prevRoad.SetPlane(withoutBonus);
                    RoadList.Add(prevRoad);
                    prevRoad.transform.parent = RoadBox;
                }
                else
                {
                    RoadPlaneCntrl road = _roadCollection.RoadFrontList[Random.Range(0, _roadCollection.RoadFrontList.Count - 1)];
                    prevRoad = Instantiate(road,
                        new Vector3(position.x + road.Scaler.x, 0, position.z),
                        Quaternion.identity);
                    prevRoad.type = 1;
                    prevFront = true;
                    prevRoad.SetPlane(withoutBonus);
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
            yield return new WaitForSeconds(2);
            GameObject roadToDel = road.gameObject;
            RoadList.Remove(road);
            Destroy(roadToDel);
           
        }
    }
}