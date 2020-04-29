using System.Threading.Tasks;
using MassTransit;

namespace RabbitMqttLib
{
    public interface IMqttService
    {
        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="exchange">query </param>
        /// <param name="message"> message</param>
        Task PushMessageAsync(string exchange, object message);

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="exchange">query</param>
        /// <param name="consumer">consumer</param>
        Task ReceiveMessageAsync<T>(string exchange, T consumer) where T : class, IConsumer;
    }
}
