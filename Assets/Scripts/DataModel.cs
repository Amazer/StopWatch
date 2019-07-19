using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DataModel :SinClass<DataModel> 
{
    public List<float> timeList;
    public float totalTime;
    public float curTime;
    public float interMin = 0.2f;
    public override void Init()
    {
        base.Init();
        timeList = new List<float>();
        totalTime = 0;
        curTime = 0;
    }
    public override void UnInit()
    {
        base.UnInit();
    }
}
