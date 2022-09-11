using System;
using System.Collections;
using System.Collections.Generic;
using TestsScript;
using Unity.VisualScripting;
using UnityEngine;
using static System.Math;

public class TestCarMoove : MonoBehaviour
{
    private TestSwipeCOntroll _swipeCOntroll;
    public float Speed;
    public float MaxSpeed;
    public int MooveType;
    public Rigidbody rb;
    public bool front = true;
    public float speed = 4;
    private float baseSpeed;
    public bool start;
    public float RotationSpeed;
    [SerializeField] private bool isRotating = false;

    public float Inertion;

    //Swap right
    private bool rightSwipe;

    private void Start()
    {
        _swipeCOntroll = GetComponent<TestSwipeCOntroll>();
        _swipeCOntroll.leftSwipe = RotateLeft;
        _swipeCOntroll.RightSwipe = RotateRight;
    }

    void test()
    {
    }

    private void FixedUpdate()
    {
        if (start && Speed < MaxSpeed)
        {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        }


        if (transform.position.y < -5)
        {
            transform.position = new Vector3(0, 0, 0);
        }

        SpeedChanger();

        //На случай проверки земли
        /*RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 5, 1))
        {
        }*/

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
    }


    public void Rotate(bool right)
    {
        rightSwipe = right;
        if (right)
        {
            StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
        }
        else
        {
            StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
        }
    }
    public void RotateRight()
    {
        rightSwipe = true;
        StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, 90, 0)));
    }
    public void RotateLeft()
    {
        rightSwipe = false;
        StartCoroutine(DoRotation(RotationSpeed, 90f, new Vector3(0, -90, 0)));
    }


    void SpeedChanger()
    {
        int y = (int) Math.Ceiling(transform.localEulerAngles.y);
        if (rightSwipe)
        {
            if (y == 1 || y > 271 && y <= 361)
            {
                MooveType = 1;
            }

            if (y > 1 && y <= 91)
            {
                MooveType = 2;
            }

            if (y > 91 && y <= 181)
            {
                MooveType = 3;
            }

            if (y > 181 && y <= 271)
            {
                MooveType = 4;
            }
        }
        else
        {
            if (y >= 270 && y < 360)
            {
                MooveType = 4;
            }

            if (y < 270 && y >= 180)
            {
                MooveType = 3;
            }

            if (y < 180 && y >= 90)
            {
                MooveType = 2;
            }

            if (y == 0 || y >= 0 && y < 90)
            {
                MooveType = 1;
            }
        }
    }

    IEnumerator DoRotation(float _speed, float amount, Vector3 axis)
    {
        isRotating = true;
        float rot = 0f;
        while (rot < amount)
        {
            yield return null;
            float delta = Mathf.Min(_speed * Time.deltaTime, amount - rot);
            transform.RotateAround(transform.position, axis, delta);
            rot += delta;
        }

        if (rightSwipe)
        {
            switch (MooveType)
            {
                case 1:
                    rb.velocity = new Vector3(-2, 0, -rb.velocity.x);
                    break;
                case 2:
                    rb.velocity = new Vector3(rb.velocity.z, 0, 2 * axis.y / 90);
                    break;
                case 3:
                    rb.velocity = new Vector3(2 * axis.y / 90, 0, -rb.velocity.x);
                    break;
                case 4:
                    rb.velocity = new Vector3(rb.velocity.z, 0, -2);
                    break;
            }
        }
        else
        {
            switch (MooveType)
            {
                case 1:
                    rb.velocity = new Vector3(-Inertion, 0, rb.velocity.x);
                    break;
                case 2:
                    rb.velocity = new Vector3(-rb.velocity.z, 0, Inertion);
                    break;
                case 3:
                    rb.velocity = new Vector3(-Inertion, 0, rb.velocity.x);
                    break;
                case 4:
                    rb.velocity = new Vector3(-rb.velocity.z, 0, Inertion);
                    break;
            }
        }

        isRotating = false;
    }
}