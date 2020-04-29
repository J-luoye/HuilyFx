using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Core.Web.Services
{
    public class ChatHouService : Hub
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string channel, string message)
        {
            await Clients.All.SendCoreAsync(channel, new object[] { message });
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public async Task SendMessageArr(string channel, object[] args)
        {
            await Clients.All.SendCoreAsync(channel, args);
        }
    }
}
