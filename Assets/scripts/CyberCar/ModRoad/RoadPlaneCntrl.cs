using System;
using System.Collections.Generic;
using System.Linq;
using CyberCar.Bonuses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CyberCar
{
    public class RoadPlaneCntrl : MonoBehaviour
    {
        public bool isStart;
        private Vector3 targetPos;
        public float speed = 10;
        public  GameObject Bonus;
        /// <summary>
        /// 1 - front;
        /// 2 = rigtht
        /// 3 - fturn
        /// 4 = rturn
        /// </summary>
        public int type;
        public void SetPlane(Vector3 _targetPos)
        {
            transform.position = _targetPos - new Vector3(0, 10, 0);
            targetPos = _targetPos;
            int bonus = Random.Range(0, 100);
            if (bonus > 0 && type != 3 && type!=4)
            {
                List<GameObject> BonusList = Resources.LoadAll<GameObject>("Bonus").ToList();  
               //Bonus = Resources.Load<GameObject>("Bonus/BonusSpeed");
               Bonus = BonusList[Random.Range(0,BonusList.Count-1)];
               GameObject Bg =Instantiate(Bonus, transform);
               Bg.transform.localPosition = Vector3.zero+new Vector3(0,0.3f,0);
               Bg.transform.localScale = new Vector3(1/transform.localScale.x, 1/transform.localScale.y, 1/transform.localScale.z);
               if (type == 2)
               {
                   Bg.transform.rotation = new Quaternion(0, 90, 0, 90);
               }
               //  bG.transform.localPosition = Vector3.zero;
            }
        }

        private void Update()
        {
            if (!isStart)
            {
                if (Vector3.Distance(transform.position, targetPos) > 0)
                {
                    var step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
                }
            }
        }
    }
}