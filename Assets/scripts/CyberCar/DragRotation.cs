using UnityEngine;
using UnityEngine.EventSystems;

namespace CyberCar
{
    public class DragRotation: MonoBehaviour
    {
        
        float rotationSpeed = 0.2f;
 
        void OnMouseDrag()
        {
            float XaxisRotation = Input.GetAxis("Mouse X")*rotationSpeed;
            transform.RotateAround (Vector3.down, XaxisRotation/10);
        }
 
      
    }
}