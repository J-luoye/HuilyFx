using System;
using System.ComponentModel.DataAnnotations;

namespace webapi.Enum
{
    /// <summary>
    /// 返回码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// token无效
        /// </summary>
        [Display(Name = "请求Token无效")]
        TokenInvalid = 5,

        /// <summary>
        /// 请求成功
        /// </summary>
        [Display(Name = "请求成功")]
        Success = 200,

        /// <summary>
        /// 请求失败
        /// </summary>
        [Display(Name = "请求失败")]
        Error = 500,
    }

    public static partial class EnumExted
    {
        /// <summary>
        /// 获取枚举字段的Display特性的名称
        /// </summary>
        /// <param name="e">枚举字段</param>
        /// <returns></returns>
        public static string GetFieldDisplay(this System.Enum e)
        {
            if (e == null)
            {
                return null;
            }

            var display = e.GetAttribute<DisplayAttribute>();
            if (display == null)
            {
                return e.ToString();
            }
            return display.Name;
        }

        /// <summary>
        /// 获取特性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static T GetAttribute<T>(this System.Enum e) where T : class
        {
            var field = e.GetType().GetField(e.ToString());
            var attribute = Attribute.GetCustomAttribute(field, typeof(T)) as T;
            return attribute;
        }
    }

}