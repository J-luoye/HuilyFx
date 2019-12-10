using StackExchange.Redis;
using System.Threading.Tasks;
using Common.Utility;

namespace Redis
{
    /// <summary>
    /// Redis List缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisListCache<T>
    {
        /// <summary>
        /// multiplexer
        /// </summary>
        private readonly Multiplexer multiplexer;

        /// <summary>
        /// 指定数据集
        /// </summary>
        private readonly int dataBase;


        #region 构造函数
        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        public RedisListCache(Multiplexer multiplexer)
        {
            this.multiplexer = multiplexer;
        }

        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        /// <param name="database">指定数据库</param>
        public RedisListCache(Multiplexer multiplexer, Database database) :
            this(multiplexer)
        {
            this.dataBase = (int)database;
        }
        #endregion

        /// <summary>
        /// 获取db
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return this.multiplexer.GetMultiplexer().GetDatabase(this.dataBase);
        }

        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="lastPop">是否删除上一次数据</param>
        /// <returns></returns>
        public async Task<T> GetAsync(string key, bool lastPop)
        {
            if (lastPop)
            {
                await this.RemoveAsync(key);
            }
            return await this.GetAsync(key);
        }


        /// <summary>
        /// 获取一条数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(string key)
        {
            var db = this.GetDatabase();
            var value = await db.ListGetByIndexAsync(key, 0);
            if (value.IsNullOrEmpty == true)
            {
                return default(T);
            }

            return JsonSerializer.TryDeserialize<T>(value);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, T value)
        {
            var db = this.GetDatabase();
            var redisValue = JsonSerializer.Serialize(value);
            await db.ListRightPushAsync(key, redisValue);
           
            return true;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            return await this.RemoveAsync(key, 1);
        }

        /// <summary>
        /// 删除指定Key中队列
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task<bool> RemoveKeyAsync(string key)
        {
            var db = this.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }

        /// <summary>
        /// 删除指定条数数据, -1 删除当前可以 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key, int count)
        {
            var db = this.GetDatabase();
            for (var i = 0; i < count; i++)
            {
                await db.ListLeftPopAsync(key);
            }
            return true;
        }
    }
}
