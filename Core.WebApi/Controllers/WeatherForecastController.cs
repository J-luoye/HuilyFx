using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMqttLib;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMqttService _mqttService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IMqttService mqttService
            )
        {
            _logger = logger;
            this._mqttService = mqttService;
        }

        [HttpGet("get")]
        public async Task Get()
        {
            //推送消息
            await _mqttService.PushMessageAsync("test", new RabbitMqOptions());
        }

        public void Test()
        {
            //接收消息
            _mqttService.ReceiveMessageAsync<MqCus>("text", new MqCus());
        }

    }
}
