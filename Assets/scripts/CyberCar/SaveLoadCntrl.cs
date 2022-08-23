using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace CyberCar
{
    public static class SaveLoadCntrl
    {
        public static CarSave LoadGame()
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