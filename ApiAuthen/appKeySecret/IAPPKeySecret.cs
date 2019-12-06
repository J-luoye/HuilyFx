using System;

namespace ApiAuthen.KeySecret
{
    /// <summary>
    /// appKey and appSecret
    /// </summary>
    public interface IAPPKeySecret
    {
        /// <summary>
        /// appKey
        /// </summary>
        string appKey { get; set; }

        /// <summary>
        /// appSecert
        /// </summary>
        string appSecret { get; set; }

    }
}
