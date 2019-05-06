using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webapi.Models
{
    /// <summary>
    /// 公用管理
    /// </summary>
    public static class authKey
    {
        /// <summary>
        /// JWT自定义密匙
        /// </summary>
        public const string secret = "我是密匙，用户自定义";
    }
}