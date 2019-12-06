using System;

namespace ApiAuthen.KeySecret
{
    /// <summary>
    /// 通过API key 和Secret 授权
    /// </summary>
    public class KeyAndSecret
    {
        /// <summary>
        /// appId
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// appSecret
        /// </summary>
        public string appSecret { get; set; }

        /// <summary>
        /// 测试
        /// </summary>
        /// <returns></returns>
        public object aa()
        {
            var key = NewKeyOrSecret.Initialization;
            return  new { key = key.appKey, secret = key.appSecret };
        }

        /// <summary>
        /// 验证key和secret是否相同
        /// </summary>
        /// <param name="key"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public bool IsAppSecret(string key, string secret)
        {
            if(string.Equals(this.appId,key) && string.Equals(this.appSecret,secret))
            {
                return true;
            }
            return false;
        }

        

    }
}
