using System.Collections.Generic;
using UnityEngine;
using System;

public class Saveable : ISaveable
{
    public List<TimeSpan> times { get; set; }

    public Saveable()
    {

    }

    public Saveable(List<TimeSpan> times)
    {
        this.times = times;
    }

    void ISaveable.PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.times = new List<double>();
        foreach (TimeSpan time in times)
        {            
            a_SaveData.times.Add(time.TotalMilliseconds);
        }
        Debug.Log(a_SaveData.times);
    }

    void ISaveable.LoadFromSaveData(SaveData a_SaveData)
    {
        times = new List<TimeSpan>();
        foreach (double time in a_SaveData.times)
        {
            times.Add(TimeSpan.FromMilliseconds(time));
        }
        Debug.Log(a_SaveData.times);
    }
}