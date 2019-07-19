using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using StopWatch;
using System.Collections.Generic;
public class StatisticsView : MonoBehaviour
{
    public Text totalTime;
    public Text interTime;
    public Text totalCount;
    public Text validCount;
    public LineChartRender chart;
    
    public void Start()
    {
    }
    private void OnEnable()
    {
        RefreshTextGroupShow();
        RefreshChartShow();
    }

    private void RefreshTextGroupShow()
    {
        FormatTime totalFt = TimeUtils.GetFormatTime(DataModel.instance.totalTime);
        totalTime.text = string.Format("{0}小时{1}分钟", totalFt.hor, totalFt.min);
        interTime.text = string.Format("{0}分钟", (int)DataModel.instance.interMin);
        totalCount.text = string.Format("{0}次", DataModel.instance.timeList.Count);
        float standardSec = DataModel.instance.interMin * 60;
        int validNum = 0;
        for(int i=0,imax=DataModel.instance.timeList.Count;i<imax;++i)
        {
            if(DataModel.instance.timeList[i]>=standardSec)
            {
                validNum++;
            }
        }

        validCount.text = string.Format("{0}次", validNum);
    }
    private void RefreshChartShow()
    {
        float interMin = DataModel.instance.interMin;
        List<float> timeList = DataModel.instance.timeList;
        float maxValue = interMin * 60f;
        for(int i=0,imax=timeList.Count;i<imax;++i)
        {
            if(timeList[i]>maxValue)
            {
                maxValue = timeList[i];
            }
        }
        float[] values = new float[timeList.Count];
        for(int i=0,imax=timeList.Count;i<imax;++i)
        {
            values[i] = timeList[i] / maxValue;
        }
        chart.values = values;
        chart.useStandardLine = true;
        chart.standardValue = interMin * 60f / maxValue;
        chart.SetVerticesDirty();

    }
}
