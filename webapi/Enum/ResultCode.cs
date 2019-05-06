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
}