using System;
using UnityEngine;

namespace CyberCar
{
    public class SwipeControll: MonoBehaviour
    {
        
//inside class
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;
        public Action RightSwipe;
        public Action leftSwipe;
        public Action UpSwipe;
        public Action DownSwipe;

        private void Update()
        {
            Swipe();
        }

        public void Swipe()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //save began touch 2d point
                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }

            if (Input.GetMouseButtonUp(0))
            {
                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
                currentSwipe.Normalize();

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    UpSwipe.Invoke();
                }

                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    DownSwipe.Invoke();
                }

                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    leftSwipe.Invoke();
                }

                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    RightSwipe.Invoke();
                }
            }
        }
    }
}