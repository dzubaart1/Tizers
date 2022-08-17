using System;

namespace CyberCar
{
    using UnityEngine;

    public class FollowCamera : MonoBehaviour
    {
        public Transform target;
        public float smooth = 5.0f;
        public Vector3 offset = new Vector3(0, 2, -5);
        public bool right;

        private void Start()
        {
            transform.parent = null;
        }

        void Update()
        {
           
                transform.position =
                    Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
           
        }
    }
}