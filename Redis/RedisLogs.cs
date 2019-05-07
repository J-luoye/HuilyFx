using StackExchange.Redis;
using System.Threading.Tasks;

namespace Redis
{
    /// <summary>
    /// 日志中心缓存
    /// </summary>
    [ConnectionString("LogsCenterData")]
    public class RedisLogs
    {
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        private readonly string name = ConnectionStringAttribute.GetConnectionName(typeof(RedisLogs));

        /// <summary>
        /// 连接管理
        /// </summary>
        private readonly Multiplexer redis;

        /// <summary>
        /// 获取唯一实例
        /// </summary>
        public static readonly RedisLogs Instance = new RedisLogs();

        /// <summary>
        /// 构造
        /// </summary>
        private RedisLogs()
        {
            this.redis = new Multiplexer(name);
        }

        /// <summary>
        /// 获取当前库
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return redis.GetMultiplexer().GetDatabase();
        }

        /// <summary>
        /// 异步添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public async Task<bool> AddAsync<T>(T data) where T : class
        {
            var Key = data.GetType().Name;
            var redisValue = data.ToRedisValue();
            var result = await this.GetDatabase().ListLeftPushAsync(Key, redisValue);
            return result > 0;
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        public bool Add<T>(T data) where T : class
        {
            var Key = data.GetType().Name;
            var redisValue = data.ToRedisValue();
            var result = this.GetDatabase().ListLeftPush(Key, redisValue);
            return result > 0;
        }
    }
}
