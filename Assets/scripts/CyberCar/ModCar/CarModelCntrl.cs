using System.Collections.Generic;
using UnityEngine;

namespace CyberCar
{
    public class CarModelCntrl : MonoBehaviour
    {
        public List<Transform> backlights;
        public Renderer _renderer;
        private List<GameObject> curBack = new List<GameObject>();

        public void setData(GameObject backlight)
        {
            if (curBack.Count > 0)
            {
                foreach (var VARIABLE in curBack)
                {
                    Destroy(VARIABLE.gameObject);
                }
            }

            curBack = new List<GameObject>();
            foreach (var trans in backlights)
            {
                curBack.Add(Instantiate(backlight, trans));
            }
        }
    }
}