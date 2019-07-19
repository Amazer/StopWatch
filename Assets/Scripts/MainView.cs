using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using StopWatch;
public class MainView : MonoBehaviour
{
    public Text total_sec;
    public Text total_mil;
    public Text cur_sec;
    public Text cur_mil;
    public Button stop;
    public Text btn_text;
    public Button lap;
    public Button reset;
    public GameObject listRoot;
    public Text listItemText;
    public List<Text> textPoolList;
    public List<Text> activeTextList;
    public Transform textPoolRoot;

    public LineChartRender chart;

    [SerializeField]
    public float interMin
    {
        get
        {
            return DataModel.instance.interMin;

        }
        set
        {
            DataModel.instance.interMin = value;
        }
    }
    public List<float> timeList
    {
        get
        {
            return DataModel.instance.timeList;
        }
    }

    private readonly int hor_mili = 1000 * 60 * 60;
    private readonly int min_mili = 1000 * 60;
    private readonly int sec_mili = 1000;
    private readonly int mili_mili = 10;

    private bool _playing = false;
    private float _totalTime
    {
        get
        {
            return DataModel.instance.totalTime;
        }
        set
        {
            DataModel.instance.totalTime = value;
        }
    }
    private float _curTime
    {
        get
        {
            return DataModel.instance.curTime;
        }
        set
        {
            DataModel.instance.curTime = value;
        }
    }
    public void Start()
    {
        textPoolList = new List<Text>();
        activeTextList = new List<Text>();
        stop.onClick.AddListener(OnClickStop);
        lap.onClick.AddListener(OnClickLap);
        reset.onClick.AddListener(OnClickRest);
        listItemText.gameObject.SetActive(false);
        listItemText.transform.parent = textPoolRoot;
        textPoolList.Add(listItemText);
        OnReset();
        RefreshBtnText();
        RefreshChartShow();
    }
    private void Update()
    {
        if(_playing)
        {
            _totalTime += Time.deltaTime;
            _curTime += Time.deltaTime;
            RefreshTimeShow();
        }
        
    }
    private void RefreshTimeShow()
    {
        total_sec.text =TimeUtils.GetMiliFormatString(_totalTime);
        cur_sec.text = TimeUtils.GetMiliFormatString(_curTime);
        
    }
    private void OnClickStop()
    {
        _playing = !_playing;
        RefreshBtnText();

    }
    private void RefreshBtnText()
    {
        if(_playing)
        {
            btn_text.text = "Stop";
        }
        else
        {
            btn_text.text = "Start";
        }
    }

    /// <summary>
    /// 更新折线图的显示
    /// </summary>
    private void RefreshChartShow()
    {
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
    private void OnClickLap()
    {
        if(!_playing)
        {
            return;
        }
        float time = _curTime;
        _curTime = 0f;
        timeList.Add(time);
        Text item;
        if(textPoolList.Count > 0)
        {
            item = textPoolList[0];
            textPoolList.RemoveAt(0);
        }
        else
        {
            item = GameObject.Instantiate(listItemText.GetComponent<Text>());
        }
        item.transform.parent = listRoot.transform;
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;
        activeTextList.Add(item);
        item.gameObject.SetActive(true);
        item.text = string.Format("{0}  {1}", timeList.Count, TimeUtils.GetMiliFormatString(time));

        RefreshChartShow();
    }
    private void OnClickRest()
    {
        _playing = false;
        OnReset();
        RefreshBtnText();
    }
    private void OnReset()
    {
        _totalTime = 0f;
        _curTime = 0f;
        RefreshBtnText();
        RefreshTimeShow();
        for(int i=0;i<activeTextList.Count;++i)
        {
            activeTextList[i].gameObject.SetActive(false);
            textPoolList.Add(activeTextList[i]);
            activeTextList[i].transform.parent = textPoolRoot;
        }
        activeTextList.Clear();
        timeList.Clear();
    }
}
