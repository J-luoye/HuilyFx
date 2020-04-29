using System;
using System.Collections.Generic;
using System.Text;
using HuilyFx.St.Common.Extends;

namespace System
{
    /// <summary>
    /// 时间扩展类
    /// </summary>
    public static partial class TimeExtend
    {
        /// <summary>
        /// 时间转换为字符串(内置常用格式)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToString(this DateTime time, string fromat = TimeFromat.STRIKEDATETIMESEC)
        {
            if(fromat == null)
            {
                return time.ToString(TimeFromat.STRIKEDATETIMESEC);
                
            }

            return time.ToString(fromat);
        }
    }
}
