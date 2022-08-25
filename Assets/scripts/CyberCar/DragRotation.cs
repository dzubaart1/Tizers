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
       //     float YaxisRotation = Input.GetAxis("Mouse Y")*rotationSpeed;
            // select the axis by which you want to rotate the GameObject
            transform.RotateAround (Vector3.down, XaxisRotation/10);
         //   transform.RotateAround (Vector3.right, YaxisRotation);
        }
 
      
    }
}