using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarGameCntrl : MonoBehaviour
{

    public GameObject RoadPrefab;
    public GameObject StartRoad;
    private GameObject prevRoad;
    void Start()
    {
        prevRoad = StartRoad;
        for (int i = 0; i < 5; i++)
        {
            int rRoad = Random.Range(0, 10);

            if (rRoad > 5)
            {
                prevRoad = Instantiate(RoadPrefab, new Vector3(prevRoad.transform.position.x , 0, prevRoad.transform.position.x + 4), Quaternion.identity);
            }
            else
            {
                prevRoad = Instantiate(RoadPrefab, new Vector3(prevRoad.transform.position.x +4, 0, prevRoad.transform.position.z), Quaternion.identity);  
            }
        }    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
