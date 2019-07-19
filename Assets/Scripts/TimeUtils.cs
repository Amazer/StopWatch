/*
********************************************** 
 *Copyright(C) 2018 by 延澈左 
 *
 *模块名:           TimeUtils.cs 
 *创建者:           延澈左 
 *创建日期:         2019-07-17 
 *修改者列表:
 *描述:    
**********************************************
*/
using UnityEngine;
using System.Collections;
namespace StopWatch
{
    public struct FormatTime
    {
        public int hor;
        public int min;
        public int sec;
        public int miliSec;
        public int totalMili;

    }
    public static class TimeUtils
    {
        private static int hor_mili = 1000 * 60 * 60;
        private static int min_mili = 1000 * 60;
        private static int sec_mili = 1000;
        private static int mili_mili = 10;
        public static string GetMiliFormatString(float miliSec)
        {
            int tmp = (int)(miliSec * 1000);

            int hor = tmp / hor_mili;
            tmp = tmp - hor_mili * hor;

            int min = tmp / min_mili;
            tmp = tmp - min_mili * min;

            int sec = tmp / sec_mili;
            tmp = tmp - sec_mili * sec;

            int mili = tmp / mili_mili;
            return string.Format("{0:d2}:{1:d2}:{2:d2}.{3:d2}", hor, min, sec, mili);
        }
        public static FormatTime GetFormatTime(float miliSec)
        {
            int tmp = (int)(miliSec * 1000);
            int totalMili = tmp;

            int hor = tmp / hor_mili;
            tmp = tmp - hor_mili * hor;

            int min = tmp / min_mili;
            tmp = tmp - min_mili * min;

            int sec = tmp / sec_mili;
            tmp = tmp - sec_mili * sec;

            int mili = tmp / mili_mili;
            FormatTime ft = new FormatTime() { hor = hor ,min=min,sec=sec,miliSec=mili,totalMili = totalMili};
            return ft;
        }
    }
}
