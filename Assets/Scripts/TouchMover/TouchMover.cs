/*
********************************************** 
 *Copyright(C) 2018 by 延澈左 
 *
 *模块名:           TouchMover.cs 
 *创建者:           延澈左 
 *创建日期:         2019-07-23 
 *修改者列表:
 *描述:    
**********************************************
*/
using UnityEngine;
using System.Collections;
namespace StopWatch
{
    public class TouchMover : MonoBehaviour
    {
        public MoveLayer[] moveLayers;
        public float elasticity = 0.2f;
        public float overStretch = 0.2f;
        public float decelerationRate = 0.135f;
        private Vector3 lastMousePos;

        [SerializeField]
        private float totalMoveX;
        [SerializeField]
        private float curMoveX;

        [SerializeField]
        private float minDisX;
        [SerializeField]
        private float maxDisX;

        [SerializeField]
        private float minDisTotalX;
        [SerializeField]
        private float maxDisTotalX;

        [SerializeField]
        private float overMinDisX;
        [SerializeField]
        private float overMaxDisX;


        private bool touched = false;
        private float refSpeed = 0f;
        private float moveSpeed = 0f;



        public void Start()
        {
            moveLayers = GetComponentsInChildren<MoveLayer>();
            MoveRange();

        }
        private void MoveRange()
        {
            minDisTotalX = 9999;
            maxDisTotalX = 9999;
            for (int i = 0, imax = moveLayers.Length; i < imax; ++i)
            {
                float min = moveLayers[i].GetMinDisX();
                float max = moveLayers[i].GetMaxDisX();
                if (min < minDisTotalX)
                {
                    minDisTotalX = min;
                }
                if (max < maxDisTotalX)
                {
                    maxDisTotalX = max;
                }
            }

            overMinDisX = minDisTotalX * overStretch;
            overMaxDisX = maxDisTotalX * overStretch;

            minDisX = minDisTotalX - overMinDisX;
            maxDisX = maxDisTotalX - overMaxDisX;

            minDisTotalX = -minDisTotalX;
            minDisX = -minDisX;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition;
                touched = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                touched = false;
            }

            if (touched)
            {
                float deltaX = (Input.mousePosition - lastMousePos).x;
                lastMousePos = Input.mousePosition;
                if (totalMoveX + deltaX < minDisTotalX)
                {
                    totalMoveX = minDisTotalX;
                    deltaX = 0f;
                }
                else if (totalMoveX + deltaX > maxDisTotalX)
                {
                    totalMoveX = maxDisTotalX;
                    deltaX = 0f;
                }
                else
                {
                    totalMoveX += deltaX;
                }
                float newSpeed = deltaX / Time.deltaTime;
                moveSpeed = Mathf.Lerp(moveSpeed, newSpeed, Time.deltaTime * 10);
            }
            else
            {
                moveSpeed = Mathf.SmoothDamp(moveSpeed, 0f, ref refSpeed, elasticity, Mathf.Infinity, Time.deltaTime);
                if (Mathf.Abs(moveSpeed) < 1)
                {
                    moveSpeed = 0f;
                }
                if (Mathf.Abs(refSpeed) < 1)
                {
                    moveSpeed = 0f;
                }
                totalMoveX += moveSpeed * Time.deltaTime;
                if (totalMoveX < minDisX)
                {
                    totalMoveX = minDisX;
                    moveSpeed = 0f;
                }
                else if (totalMoveX > maxDisX)
                {
                    totalMoveX = maxDisX;
                    moveSpeed = 0f;
                }
            }
            if (Mathf.Abs(curMoveX - totalMoveX) > 0.0001f)
            {
                curMoveX = totalMoveX;
                for (int i = 0, imax = moveLayers.Length; i < imax; ++i)
                {
                    moveLayers[i].MoveTo(curMoveX);
                }
            }

        }

        public void ResetToInit()
        {
            totalMoveX = 0;
            curMoveX = 0;
            touched = false;
            refSpeed = 0;
            moveSpeed = 0;
            for (int i = 0, imax = moveLayers.Length; i < imax; ++i)
            {
                moveLayers[i].ResetToInit();
            }
        }
    }
}
