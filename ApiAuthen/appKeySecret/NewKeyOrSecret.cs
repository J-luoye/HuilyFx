using System;

namespace ApiAuthen.KeySecret
{
    /// <summary>
    /// 生成授权信息
    /// </summary>
    public class NewKeyOrSecret : IAPPKeySecret
    {
        /// <summary>
        /// 获取新授权
        /// </summary>
        public static readonly NewKeyOrSecret Initialization = new NewKeyOrSecret();

        /// <summary>
        /// appKey
        /// </summary>
        public string appKey { get; set; }
        /// <summary>
        /// appSecret
        /// </summary>
        public string appSecret { get; set; }

        public NewKeyOrSecret()
        {
            NewAppKey();
        }

        /// <summary>
        /// 生成appKey和appSecret
        /// </summary>
        private void NewAppKey()
        {
            appKey = Guid.NewGuid().ToString().Replace("-","");
            appSecret = Guid.NewGuid().ToString().Replace("-", "");
        }

    }
}
