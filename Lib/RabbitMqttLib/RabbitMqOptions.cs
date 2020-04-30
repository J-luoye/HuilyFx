namespace RabbitMqttLib
{
    /// <summary>
    /// Options
    /// </summary>
    public class RabbitMqOptions
    {
        /// <summary>
        ///  MQHost
        /// </summary>
        public string ServerHost { get; set; } = "http://localhost:15672";

        /// <summary>
        /// 账号
        /// </summary>
        public string User { get; set; } = "guest";

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; } = "guest";
    }
}
