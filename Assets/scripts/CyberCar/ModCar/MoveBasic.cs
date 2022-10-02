using UnityEngine;

namespace CyberCar
{
    public abstract class MoveBasic: MonoBehaviour
    {
        private bool OnAGround;
        public abstract void Moove();
        public abstract void  Nitro(bool _onNitro);

        public abstract void BrakeTorque(bool inBrake);

        public  void CheckInfo()
        {
            RaycastHit hit;
            RaycastHit hitGround;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down) , out hitGround,
                1,
                1))
            {
                OnAGround = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.blue);
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

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.magenta);
        }
    }
}