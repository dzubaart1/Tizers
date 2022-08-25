using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using CyberCar.ModShopItems;
using UnityEngine;
namespace CyberCar.MenuCntrls
{
    public class ShopCntrl
    {
        public CarParams CurCar { get; set; }
        public int CurColor { get; set; }
        public int CurBacklight { get; set; }

        public  List<CarParams> GetCarsList()
        {
            List<CarParams> CarsObjs = Resources.LoadAll<CarParams>("CustomCars").ToList();
            return CarsObjs;
        }  
        public  List<ColorItem> GetColorsList()
        {
            List<ColorItem> Colors = Resources.LoadAll<ColorItem>("ColorItems").ToList();
            return Colors;
        } 
        public  List<BackLightItem> GetBackLists()
        {
            List<BackLightItem> BackLists = Resources.LoadAll<BackLightItem>("BackLightsItems").ToList();
            return BackLists;
        } 
        public void SaveGame()
        {
            BinaryFormatter bf = new BinaryFormatter(); 
            FileStream file = File.Create(Application.persistentDataPath + "/MySaveData.dat"); 
            CarSave data = new CarSave();
            data.carParamsId = CurCar.id;
            data.colorId = CurColor;
            data.backlightsId = CurBacklight;
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game data saved!");
        }
        
       public  CarSave LoadGame()
        {
            if (File.Exists(Application.persistentDataPath  + "/MySaveData.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = 
                    File.Open(Application.persistentDataPath 
                              + "/MySaveData.dat", FileMode.Open);
                CarSave data = (CarSave)bf.Deserialize(file);
                file.Close();
                return data;
            }
            else
                return null;
        }

         
    }
}