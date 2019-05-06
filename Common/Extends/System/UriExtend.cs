using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// Uri扩展
    /// </summary>
    public static partial class UriExtend
    {
        /// <summary>
        /// 合并URL
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="relativeUri">相对地址</param>
        /// <returns></returns>
        public static Uri Combine(this Uri baseUrl, string relativeUri)
        {
            if (baseUrl == null || baseUrl.IsAbsoluteUri == false)
            {
                throw new NotSupportedException();
            }
            Uri fullUrl;
            Uri.TryCreate(baseUrl, relativeUri, out fullUrl);
            return fullUrl;
        }

        /// <summary>
        /// 更换scheme
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="scheme">新的scheme</param>
        /// <returns></returns>
        public static Uri ChangeScheme(this Uri uri, string scheme)
        {
            if (string.IsNullOrEmpty(scheme))
            {
                return uri;
            }

            var value = Regex.Replace(uri.ToString(), @"^" + uri.Scheme, scheme);
            return new Uri(value, UriKind.RelativeOrAbsolute);
        }

        /// <summary>
        /// 更换host
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="host">新的host</param>
        /// <returns></returns>
        public static Uri ChangeHost(this Uri uri, string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return uri;
            }

            var value = Regex.Replace(uri.ToString(), "(?<=//)" + uri.Host, host);
            return new Uri(value, UriKind.RelativeOrAbsolute);
        }


        /// <summary>
        /// 设置参数值
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        /// <returns></returns>
        public static Uri SetParameter(this Uri uri, string parameterName, string parameterValue)
        {
            parameterValue = System.Web.HttpUtility.UrlEncode(parameterValue);
            var kvs = uri.Query.TrimStart('?')
                .Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Split('='))
                .Select(item => new { k = item.First(), v = item.Length == 2 ? item.Last() : null });

            if (kvs.Any(kv => string.Equals(kv.k, parameterName, StringComparison.OrdinalIgnoreCase)) == false)
            {
                var newKv = new { k = parameterName, v = parameterValue };
                kvs = kvs.Concat(new[] { newKv });
            }

            var newKvs = from kv in kvs
                         let v = string.Equals(kv.k, parameterName, StringComparison.OrdinalIgnoreCase) ? parameterValue : kv.v
                         select string.Format("{0}={1}", kv.k, v);

            var query = "?" + string.Join("&", newKvs);
            return new Uri(uri, uri.AbsolutePath + query);
        }
    }
}
