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


        /// <summary> 
        /// 表示成功
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static ResultModel Success(object Data = null)
        {
            return new ResultModel() { code = ResultCode.Success, Message = ResultCode.Success.GetFieldDisplay(), Data = Data };
        }

        /// <summary>
        /// 表示成功,自定义消息
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel Success(object Data, string message)
        {
            return new ResultModel() { code = ResultCode.Error, Message = message, Data = Data };
        }

        /// <summary>
        /// 表示失败
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public static ResultModel Error(object Data)
        {
            return new ResultModel() { code = ResultCode.Error, Message = ResultCode.Error.GetFieldDisplay(), Data = Data };
        }

        /// <summary>
        /// 表示失败,自定义消息
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel Error(object Data, string message)
        {
            return new ResultModel() { code = ResultCode.Error, Message = message, Data = Data };
        }

        /// <summary>
        /// 自定义返回结果
        /// </summary>
        /// <param name="code"></param>
        /// <param name="Data"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static ResultModel Custom(ResultCode code, object Data, string message)
        {
            return new ResultModel() { code = code, Message = message, Data = Data };
        }
    }
}