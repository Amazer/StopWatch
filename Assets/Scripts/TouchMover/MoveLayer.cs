/*
********************************************** 
 *Copyright(C) 2018 by 延澈左 
 *
 *模块名:           MoveLayer.cs 
 *创建者:           延澈左 
 *创建日期:         2019-07-23 
 *修改者列表:
 *描述:    
**********************************************
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace StopWatch
{
    public class MoveLayer : MonoBehaviour 
    {
        public float speed;
        private float hw;
        private RectTransform rt;
        [SerializeField]
        private float posX;
        [SerializeField]
        private Vector3 origPos;
        [SerializeField]
        private float maxPosX;
        [SerializeField]
        private float minPosX;
        public void Awake()
        {
            rt = GetComponent<RectTransform>();
            hw = rt.sizeDelta.x * 0.5f;
            posX = rt.localPosition.x;
            origPos = rt.localPosition;
            float deltaSize = hw - 360;// 假设屏幕宽度是720
            minPosX = -deltaSize;
            maxPosX = deltaSize;
        }

        public float GetMinDisX()
        {
            return (posX - minPosX) / speed;
        }
        public float GetMaxDisX()
        {
            return (maxPosX - posX) / speed;
        }
        public void MoveTo(float tarX)
        {
            Vector3 pos = rt.localPosition;
            float tarLen = tarX * speed;
            pos.x += tarLen;
            if(pos.x>maxPosX)
            {
                pos.x = maxPosX;
            }
            else if(pos.x < minPosX)
            {
                pos.x = minPosX;
            }
            rt.localPosition = pos;
        }
        public void ResetToInit()
        {
            rt.localPosition = origPos;
        }
    }
}
