using System;
using webapi.Enum;

namespace webapi.Models
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        public ResultCode code { get; set; }

        /// <summary>
        /// 返回消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }
    }
}