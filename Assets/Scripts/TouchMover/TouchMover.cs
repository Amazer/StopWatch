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

        public float delayScaler = 1f;

        [SerializeField]
        private bool bounce = false;

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

        [SerializeField]
        private float _pixelScaler = 0f;    // 由于分辨率的不同，需要进行横向移动距离的缩放，以得到实际UI应该移动的距离
        private int screenWidth = 0;

        private float pixelScaler
        {
            get
            {
                if (screenWidth != Screen.width)
                {
                    screenWidth = Screen.width;
                    _pixelScaler = 720f / screenWidth;
                }
                return _pixelScaler;
            }
        }


        private bool touched = false;
        [SerializeField]
        private float refSpeed = 0f;
        [SerializeField]
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
                deltaX = deltaX * pixelScaler * delayScaler;
                lastMousePos = Input.mousePosition;
                float tmpMoveX = totalMoveX + deltaX;
                if (tmpMoveX > maxDisX || tmpMoveX < minDisX)
                {
                    float tarTotalMoveX = totalMoveX + deltaX * 0.2f;
                    deltaX = tarTotalMoveX - totalMoveX;
                    totalMoveX = tarTotalMoveX;
                    bounce = true;
                }
                else
                {
                    totalMoveX = tmpMoveX;
                    bounce = false;
                }

                if (totalMoveX < minDisTotalX)
                {
                    totalMoveX = minDisTotalX;
                    touched = false;
                    bounce = true;
                }
                else if (totalMoveX > maxDisTotalX)
                {
                    totalMoveX = maxDisTotalX;
                    touched = false;
                    bounce = true;
                }

                float newSpeed = deltaX / Time.deltaTime;
                moveSpeed = Mathf.Lerp(moveSpeed, newSpeed, Time.deltaTime * 5);
            }
            else
            {
                if (bounce)
                {
                    if (totalMoveX < minDisX)
                    {
                        moveSpeed = 0f;
                        //                        totalMoveX = Mathf.SmoothDamp(totalMoveX, minDisX, ref refSpeed, elasticity, Mathf.Infinity, Time.deltaTime);
                        totalMoveX = totalMoveX + 500 * Time.deltaTime; 
                        if(totalMoveX>minDisX)
                        {
                            totalMoveX = minDisX; 
                        }

                    }
                    else if (totalMoveX > maxDisX)
                    {
                        moveSpeed = 0f;
//                        totalMoveX = Mathf.SmoothDamp(totalMoveX, maxDisX, ref refSpeed, elasticity, Mathf.Infinity, Time.deltaTime);
                        totalMoveX = totalMoveX - 500 * Time.deltaTime; 
                        if(totalMoveX<maxDisX)
                        {
                            totalMoveX = maxDisX; 
                        }
                    }
//                    if (Mathf.Abs(refSpeed) < 1)
//                    {
//                        refSpeed = 0f;
//                    }

                }
                else
                {
                    if (moveSpeed != 0f)
                    {
                        moveSpeed = moveSpeed * Mathf.Pow(decelerationRate, Time.deltaTime);
                        if (Mathf.Abs(moveSpeed) < 50)
                        {
                            moveSpeed = 0f;
                        }
                        if (Mathf.Abs(refSpeed) < 1)
                        {
                            refSpeed = 0f;
                        }
                        float lastMoveX = totalMoveX;
                        totalMoveX += moveSpeed * Time.deltaTime;
                        if (totalMoveX < minDisTotalX)
                        {
                            moveSpeed = 0f;
                            totalMoveX = minDisTotalX;
                        }
                        else if (totalMoveX > maxDisTotalX)
                        {
                            moveSpeed = 0f;
                            totalMoveX = maxDisTotalX;
                        }

                        if (totalMoveX < minDisX || totalMoveX > maxDisX)
                        {
                            //                            totalMoveX = Mathf.SmoothDamp(lastMoveX, totalMoveX, ref refSpeed, elasticity, Mathf.Infinity, Time.deltaTime);
                            totalMoveX = lastMoveX + moveSpeed * Time.deltaTime * 0.2f;
                            moveSpeed *= 0.8f;
                        }

                        if (moveSpeed == 0f)
                        {
                            bounce = true;
                        }
                    }
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
            bounce = false;
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
