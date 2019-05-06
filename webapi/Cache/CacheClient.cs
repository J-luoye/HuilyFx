using System;
using System.Web;

namespace webapi.Cache
{
    /// <summary>
    /// cache缓存
    /// </summary>
    public class CacheClient
    {
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T GetModel<T>(string key)
        {
            return (T)HttpRuntime.Cache.Get(key);
        }

        /// <summary>
        /// 插入缓存
        /// </summary>
        /// <param name="key">设置缓存的key值</param>
        /// <param name="value">设置缓存的value值</param>
        /// <param name="absoluteExpiration">缓存过期时间</param>
        public static void CacheInsert(string key, object value, DateTime absoluteExpiration)
        {
            HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, TimeSpan.Zero);
        }
    }
}