using System;
using System.Collections.Generic;
using System.Linq;
using CyberCar.Bonuses;
using TestsScript;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class RoadPlaneCntrl : MonoBehaviour
    {
        public bool isStart;
        public float speed = 10;
        public bool IsFinish;
        public GameObject Bonus;
        public GameObject Obstacle;
        public Vector3 Scaler = new Vector3(30, 0, 30);
        private Rigidbody rb;
        [Header("obtacles and bonus")] 
        public GameObject obtacle;
        public GameObject bonus;
        

        /// <summary>
        /// 1 - front;
        /// 2 = rigtht
        /// 3 - fturn
        /// 4 = rturn
        /// </summary>
        public int type;

        public void SetPlane()
        {
            rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            //transform.position = _targetPos - new Vector3(0, 10, 0);
            if (!obtacle && !bonus)
            {
                GenerateRandomObject();
            }
            else
            {
                if (obtacle)
                {
                    GameObject bg = Instantiate(obtacle, transform);
                    bg.transform.localPosition = Vector3.zero + new Vector3(0, 0.3f, 0);
                    bg.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y,
                        1 / transform.localScale.z);
                    if (type == 2)
                    {
                        bg.transform.rotation = new Quaternion(0, 90, 0, 90);
                    }
                }
                else
                {
                    GameObject bg = Instantiate(bonus, transform);
                    bg.transform.localPosition = Vector3.zero + new Vector3(0, 0.3f, 0);
                    bg.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y,
                        1 / transform.localScale.z);
                    if (type == 2)
                    {
                        bg.transform.rotation = new Quaternion(0, 90, 0, 90);
                    }
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            RoadCntrl.Instance.DestroyRoad(this);
        }
        void GenerateRandomObject()
        {
            int bonus = Random.Range(0, 100);
            int obtain = Random.Range(0, 100);
            if (bonus > obtain)
            {
                if (bonus > 60 && type != 3 && type != 4)
                {
                    List<GameObject> BonusList = Resources.LoadAll<GameObject>("Bonus").ToList();
                    //Bonus = Resources.Load<GameObject>("Bonus/BonusSpeed");
                    Bonus = BonusList[Random.Range(0, BonusList.Count - 1)];
                    GameObject Bg = Instantiate(Bonus, transform);
                    Bg.transform.localPosition = Vector3.zero + new Vector3(0, 0.3f, 0);
                    Bg.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y,
                        1 / transform.localScale.z);
                    if (type == 2)
                    {
                        Bg.transform.rotation = new Quaternion(0, 90, 0, 90);
                    }
                    //  bG.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                if (obtain > 40 && type != 3 && type != 4)
                {
                    List<GameObject> ObstacleList = Resources.LoadAll<GameObject>("Obstacles").ToList();
                    //Bonus = Resources.Load<GameObject>("Bonus/BonusSpeed");
                    Obstacle = ObstacleList[Random.Range(0, ObstacleList.Count)];
                    GameObject Bg = Instantiate(Obstacle, transform);
                    Bg.transform.localPosition = Vector3.zero + new Vector3(0, 0.3f, 0);
                    Bg.transform.localScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y,
                        1 / transform.localScale.z);
                    if (type == 2)
                    {
                        Bg.transform.rotation = new Quaternion(0, 90, 0, 90);
                    }
                }
            }
        }
        
        
    }
   
}