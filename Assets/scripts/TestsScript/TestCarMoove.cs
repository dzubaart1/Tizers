using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestCarMoove : MonoBehaviour
{
    public float Speed;
    public float MaxSpeed;
    public int MooveType;
    public Rigidbody rb;
    public bool front = true;
    public float speed = 4;
    private float baseSpeed;
    public bool start;
    public float RotationSpeed;
    
    //Swap right
    private bool rightSwipe;
    
   [SerializeField] private bool isRotating = false;
    private Coroutine rotatingCoro;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            
            if (!start) start = true;
            else front = !front;
        }

        switch (MooveType)
        {
            case 1:
                Speed = rb.velocity.z;
                break; 
            case 2:
                Speed = rb.velocity.x;
                break;
            case 3:
                Speed = -rb.velocity.z;
                break;
            case 4:
                Speed = -rb.velocity.x;
                break;
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)&& !isRotating)
        {
            rightSwipe = true;
            StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0,90,0)));
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)&& !isRotating)
        {
            rightSwipe = false;
           StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0,-90,0)));
        }

    }

    public void Rotate(bool right )
    {
        rightSwipe = right;
        if (right)
        {
            StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0,90,0))); 
        }
        else
        {
            StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0,-90,0)));
        }
        
    }


    void SpeedChanger()
    {
        int y =(int) Math.Ceiling(transform.localEulerAngles.y);
     if(rightSwipe)
     {
        if (y==1 || y>271 && y<=361  )
        {
            MooveType = 1;
        }
        if (y>1 && y<=91)
        {
            MooveType = 2;
        }
        if (y>91 && y<=181)
        {
            MooveType = 3;
        }
        if (y>181&&y<=271)
        {
            MooveType = 4;
        }
     }
     else
     {
         if (y>=270 && y<360  )
         {
             MooveType = 4;
         }
         if (y<270 && y>=180)
         {
             MooveType = 3;
         }
         if (y<180 && y>=90)
         {
             MooveType = 2;
         }
        
         if (y==0 ||  y>=0 && y<90)
         {
             MooveType = 1;
         }
     }

    }
    private void FixedUpdate()
    {
        if (start && Speed<MaxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * speed,ForceMode.Acceleration);
        }

        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 0, 0);
            StopRotating();
        }
        SpeedChanger();

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5, 1))
        {
        }
        
    }
    IEnumerator DoRotation(float _speed, float amount, Vector3 axis)
    {
        rb.drag = 0.5f;
        //rb.velocity = Vector3.zero;
        //rb.angularVelocity = Vector3.zero;
        isRotating = true;
        float rot = 0f;
        while (rot < amount)
        {
            yield return null;
            float delta = Mathf.Min(_speed * Time.deltaTime, amount - rot);
            transform.RotateAround(transform.position, axis, delta);
           


            rot += delta;
        }
        rb.drag = 0;
        isRotating = false;
        rotatingCoro = null;
        
        
    }

    void StopRotating()
    {
        if (rotatingCoro != null) 
        {
            StopCoroutine(rotatingCoro);
            rotatingCoro = null;
        }
    }
}