using System.Collections.Generic;
using UnityEngine;

public static class SaveDataManager
{
    public static void SaveJsonDatas(IEnumerable<ISaveable> a_Saveables)
    {
        SaveData sd = new SaveData();
        foreach(var saveable in a_Saveables)
        {
            saveable.PopulateSaveData(sd);
        }

        if(FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Successful");
        }
    }

    public static void LoadJsonDatas(IEnumerable<ISaveable> a_Saveables)
    {
        if(FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            foreach(var saveable in a_Saveables)
            {
                saveable.LoadFromSaveData(sd);
            }

            Debug.Log("Load complete");
        }
    }

    public static void SaveJsonData(ISaveable a_Saveables)
    {
        SaveData sd = new SaveData();
        a_Saveables.PopulateSaveData(sd);
        

        if (FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Successful");
        }
    }

    public static void LoadJsonData(ISaveable a_Saveables)
    {
        if (FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);
            a_Saveables.LoadFromSaveData(sd);

            Debug.Log("Load complete");
        }
    }
}