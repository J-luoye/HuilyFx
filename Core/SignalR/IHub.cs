using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.SignalR
{
    public interface IHub
    {
        /// <summary>
        /// 所有客户端发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage<T>(T message);

        /// <summary>
        /// 所有客户端发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendMessage<T>(T message, params object[] args);
    }
}
