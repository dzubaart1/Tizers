using System;
using UnityEngine;

namespace CyberCar
{
    public abstract class MoveBasic: MonoBehaviour
    {

        [Header("Main params")]
        public CarCntrl Cntrl;
        public bool OnAGround;
        public bool canMoove;
        public bool onNitro;
        public bool onBrake;
        public bool start;
        public float AccelerationSpeed = 4;

        public GameObject DriftWheels;
        public GameObject DefaultWheels;
        
        [Header("Controll params")]
        public float CurSpeed;
        public float MaxSpeed;
        public float MaxNitroSpeed;
        public float maxBrake = 50;
        
        public Vector3 SavedVelocity;
        public bool lightIsEnable;
        public abstract void Moove();
        public abstract void  Nitro(bool _onNitro);

        public abstract void BrakeTorque(bool inBrake);

         void EnableBackLightTrail()
        {
            if (CurSpeed >= MaxSpeed - 5 && !lightIsEnable)
            {
                foreach (var lights in Cntrl.BackLightsItems)
                {
                    lights.SetActive(true);
                }

                lightIsEnable = true;
            }
            
            else if (CurSpeed <= MaxSpeed - 5 && lightIsEnable)
            {
                lightIsEnable = false;
                foreach (var lights in Cntrl.BackLightsItems)
                {
                    lights.SetActive(false);
                }
            }
        }


        public  void CheckInfo()
        {
            RaycastHit hit;
            RaycastHit hitGround;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) , out hitGround,
                2,
                1))
            {
                OnAGround = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 200, Color.red);
                OnAGround = false;
            }

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) * 1000, out hit,
                Mathf.Infinity,
                1))
            {
                if (hit.transform.parent && hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>() != null)
                {
                    RoadPlaneCntrl road = hit.transform.parent.gameObject.GetComponent<RoadPlaneCntrl>();
                    road.PingDestroy();
                }
                else
                {
                    if (hit.transform.gameObject.GetComponent<RoadPlaneCntrl>() != null)
                    {
                        RoadPlaneCntrl road = hit.transform.gameObject.GetComponent<RoadPlaneCntrl>();

                        road.PingDestroy();
                    }
                }
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.red);
            }

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * 10, Color.magenta);
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * 2, Color.magenta);

            Quaternion rotationR= Quaternion.AngleAxis(7, Vector3.up);
            Quaternion rotationL= Quaternion.AngleAxis(-7, Vector3.up);
            Debug.DrawRay(transform.position, rotationR * Vector3.forward * 40, Color.magenta);
            Debug.DrawRay(transform.position, rotationL * Vector3.forward * 40, Color.magenta);
            Debug.DrawRay(transform.position, Vector3.forward * 40, Color.magenta);

            //Check backLights
            EnableBackLightTrail();
        }
    }
}