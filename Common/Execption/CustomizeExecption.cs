using System;

namespace Common.Execption
{
    /// <summary>
    /// 自定义异常
    /// </summary>
    [Serializable]
    public class CustomizeExecption : SystemException
    {
        /// <summary>
        /// 异常消息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }

        public CustomizeExecption()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">异常消息</param>
        public CustomizeExecption(string message): base(message)
        {
            this.ErrorMessage = message;
            this.Exception = base.InnerException;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="exception">exception</param>
        public CustomizeExecption(string message, Exception exception)
            :base(message, exception)
        {
            this.ErrorMessage = message;
            this.Exception = base.InnerException;
        }

    }
}
