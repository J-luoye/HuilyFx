using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SigalRLib
{
    public abstract class ChatHub : Hub
    {
        /// <summary>
        /// 所有客户端发送消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage<T>(T message)
        {
            await Clients.All.SendAsync(nameof(T), $"{Context.ConnectionId}:{ JsonSerializer.Serialize(message)}");
        }

        /// <summary>
        ///  所有客户端发送消息 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        public async Task SendMessage<T>(T message, params object[] args)
        {
            await Clients.All.SendCoreAsync(nameof(T), args);
        }

    }
}
