using System.Collections.Generic;
using System.Linq;
using CyberCar;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CyberCarMenuCntrl 
{
    public void RunGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("CyberCar");
    }

    public  List<CarParams> GetCarsList()
    {
        List<CarParams> CarsObjs = Resources.LoadAll<CarParams>("CustomCars").ToList();
        return CarsObjs;
    }
}
