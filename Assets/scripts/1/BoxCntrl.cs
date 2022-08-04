using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BoxCntrl : MonoBehaviour
{
    public  Rigidbody rb;
    public int MyValue =2;
    public List<TMP_Text> Texts;
    public bool isActive;
    public int MyColor;
    private void Start()
    {
        SetTexts();
    }

    public void dropBox()
    {
        transform.parent = null;
        isActive = true;
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<BoxCntrl>() != null)
        {
            BoxCntrl oth = other.gameObject.GetComponent<BoxCntrl>();
            if (oth.MyValue == MyValue && oth.isActive)
            {
                Destroy(other.gameObject);
                MyValue *= 2;
                SetTexts();
                SceneManager.Instance.setScore(MyValue);
                SceneManager.Instance.audio.Play();
                MyColor++;
                GetComponent<Renderer>().material.color = SceneManager.Instance.ColorsDictionary[MyColor];
            }
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.GetComponent<BoxCntrl>() != null)
        {
            BoxCntrl oth = other.gameObject.GetComponent<BoxCntrl>();
            if (oth.MyValue == MyValue && oth.isActive)
            {
                Destroy(other.gameObject);
                MyValue *= 2;
                SetTexts();
                SceneManager.Instance.setScore(MyValue);
                MyColor++;
                GetComponent<Renderer>().material.color = SceneManager.Instance.ColorsDictionary[MyColor];
            }
        }
    }

    void SetTexts()
    {
        foreach (var VARIABLE in Texts)
        {
            VARIABLE.text = MyValue.ToString();
        }
    }
}
