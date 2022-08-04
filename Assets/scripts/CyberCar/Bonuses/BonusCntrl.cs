using System;
using System.Collections;
using UnityEngine;

namespace CyberCar.Bonuses
{
    public class BonusCntrl : MonoBehaviour
    {
        public GameObject DestroyEfect;
        [SerializeField] public BonusEfect Effect = new BonusEfect();

        public void GetBonus()
        {
            if (!Effect.AddScore)
            {
                DestroyEfect.SetActive(true);
                GetComponent<MeshRenderer>().enabled = false;
                StartCoroutine(DestroyByTime());
            }
            else
            {
                GetComponent<BoxCollider>().enabled = false;
                transform.Find("coin").transform.Find("coin").GetComponent<MeshRenderer>().enabled = false;
                DestroyEfect.SetActive(true);
                StartCoroutine(DestroyByTime());
            }
        }

        IEnumerator DestroyByTime()
        {
            yield return new WaitForSeconds(20);
            Destroy(gameObject);
        }
    }
}