namespace Redis
{
    /// <summary>
    /// 数据库枚举
    /// </summary>
    public enum Database
    {
        /// <summary>
        /// 公有云Token和缓存 0
        /// </summary>
        Token = 0,

        /// <summary>
        /// 公安局队列 1
        /// </summary>
        AcsQuque = 1,

        /// <summary>
        /// 私有云子系统数据同步 2
        /// </summary>
        PR_DataSyc = 2,

        /// <summary>
        /// 云之讯用户账号 3
        /// </summary>
        UcsUser = 3,

        /// <summary>
        /// 门口机增量数据
        /// </summary>
        RKE_DataSync = 4,

        /// <summary>
        /// 随机密码
        /// </summary>
        RandomPwd = 5,

        /// <summary>
        /// 订阅终端对应的发布服务
        /// </summary>
        SubClients = 6,

        /// <summary>
        /// 支付宝数据缓存
        /// </summary>
        AlipayData = 7,

        /// <summary>
        /// 电梯机增量数据
        /// </summary>
        ElevDataSync = 8,

        /// <summary>
        /// 天气数据
        /// </summary>
        WeatherData = 9,

        /// <summary>
        /// 微信缓存数据
        /// </summary>
        WeiXinData = 10,
    }
}
