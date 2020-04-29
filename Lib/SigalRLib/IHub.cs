using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SigalRLib
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
