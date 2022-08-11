using System.Collections.Generic;
using UnityEngine;

namespace CyberCar
{
    public class CarModelCntrl : MonoBehaviour
    {
        public List<Transform> backlights;
        public Renderer _renderer;

        public void setData(GameObject backlight, Texture tex)
        {
            _renderer.material.mainTexture = tex;
            foreach (var trans in backlights)
            {
                Instantiate(backlight, trans);
            }
        }

    }
}