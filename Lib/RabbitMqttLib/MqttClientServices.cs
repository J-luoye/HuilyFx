using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Options;

namespace RabbitMqttLib
{
    public class MqttClientServices : IMqttService
    {
        private readonly RabbitMqOptions _options;

        public MqttClientServices(IOptions<RabbitMqOptions> options)
        {
            this._options = options.Value;
        }


        /// <summary>
        /// 创建连接对象
        /// </summary>
        private IBusControl CreateBus(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            //通过MassTransit创建MQ联接工厂
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(_options.ServerHost), hst =>
                {
                    hst.Username(_options.User);
                    hst.Password(_options.Password);
                });
                registrationAction?.Invoke(cfg, host);
            });
        }


        /// <summary>
        /// 推送消息
        /// 这里使用fanout的交换类型(发布/订阅)
        /// </summary>
        ///<param name="exchange">队列名称</param>
        ///<param name="obj">消息内容</param>
        public async Task PushMessageAsync(string exchange, object message)
        {
            var bus = CreateBus();
            var sendToUri = new Uri($"{_options.ServerHost}/{exchange}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            await endPoint.Send(message);
        }

        /// <summary>
        /// 接收消息
        /// 这里使用fanout的交换类型（发布/订阅）
        /// consumer必需是实现IConsumer接口的类实例
        /// </summary>
        ///<param name="exchange"></param>
        ///<param name="consumer"></param>
        public async Task ReceiveMessageAsync<T>(string exchange, T consumer) where T : class, IConsumer
        {
            var bus = CreateBus((cfg, host) =>
            {
                //从指定的消息队列获取消息 通过consumer来实现消息接收
                cfg.ReceiveEndpoint(exchange, e =>
                {
                    e.Instance(consumer);
                });
            });
            await bus.StartAsync();
        }
    }
}