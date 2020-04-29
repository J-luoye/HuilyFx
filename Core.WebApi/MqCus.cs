using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using RabbitMqttLib;

namespace Core.WebApi
{
    public class MqCus : IConsumer<RabbitMqOptions>
    {

        public Task Consume(ConsumeContext<RabbitMqOptions> context)
        {
            return Task.Run(() => GetMessageAsync(context));
        }


        public async Task GetMessageAsync(ConsumeContext<RabbitMqOptions> context)
        {
            var message = context.Message;
            Console.WriteLine("接收消息：" + System.Text.Json.JsonSerializer.Serialize(message));
        }

    }
}
