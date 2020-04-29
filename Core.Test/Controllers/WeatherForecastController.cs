using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RabbitMqttLib;

namespace Core.Test.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMqttService _mqttService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, 
            IMqttService mqttService)
        {
            _logger = logger;
            this._mqttService = mqttService;
        }

        [HttpGet]
        public void Get()
        {
            //接收消息
            _mqttService.ReceiveMessageAsync<MqConsume>("test", new MqConsume());
        }
    }
}
