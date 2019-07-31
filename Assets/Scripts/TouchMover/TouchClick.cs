/*
********************************************** 
 *Copyright(C) 2018 by 延澈左 
 *
 *模块名:           TouchClick.cs 
 *创建者:           延澈左 
 *创建日期:         2019-07-31 
 *修改者列表:
 *描述:    
**********************************************
*/
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace StopWatch
{
    public class TouchClick : MonoBehaviour
    {
        public Camera uiCam;
        private RectTransform rt;
        private Transform tr;
        private Vector3 startPos;
        private Vector3 lastPos;
        private Vector3 deltaPos;
        private float deltaDis;
        private bool canCheckClick = false;
        public void Start()
        {
            rt = GetComponent<RectTransform>();
            tr = GetComponent<Transform>();
            uiCam = GameObject.Find("UICamera").GetComponent<Camera>();
        }
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPosOnButton(Input.mousePosition))
                {
                    canCheckClick = true;
                    startPos = Input.mousePosition;
                    lastPos = Input.mousePosition;
                    deltaDis = 0f;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (canCheckClick)
                {
                    if (IsPosOnButton(Input.mousePosition))
                    {
                        if (deltaDis < 10)
                        {
                            // 触发onclick
                            Debug.Log("Touch OnClick Btn,deltaDis:{0}",deltaDis);

                        }
                        else
                        {
                            Debug.Log("Touch deltaDis:{0}",deltaDis);
                        }
                    }
                }
                canCheckClick = false;
            }
            if (canCheckClick)
            {
                deltaPos = Input.mousePosition - lastPos;
                deltaDis += deltaPos.magnitude;
                lastPos = Input.mousePosition;
            }

        }
        private bool IsPosOnButton(Vector3 screenPos)
        {
            Vector3 worldPos = uiCam.ScreenToWorldPoint(screenPos);
            Vector3 localPos = tr.InverseTransformPoint(worldPos);
            if (rt.rect.Contains(localPos))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
