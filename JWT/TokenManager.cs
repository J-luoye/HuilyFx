using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using System;
using System.Collections.Generic;

namespace JwtDotnet
{
    public class TokenManager
    {
        /// <summary>
        /// key
        /// </summary>
        public string secret { get; set; }

        public TokenManager(string secret)
        {
            this.secret = secret;
        }

        /// <summary>
        /// 创建token,这种方式生成的token是固定的，只要payload没有改变，token就不会改变
        /// </summary>
        /// <param name="payload">自定义返回数据</param>
        /// <param name="secret">密匙</param>
        /// <returns></returns>
        public string CreateToken(object payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);
            return token;
        }

        /// <summary>
        /// 创建token,注意要解析token是一个匿名方式的对象，格式new T{ exp, data }
        /// </summary>
        /// <param name="payload">自定义数据</param>
        /// <param name="month">过期时间,单位min</param>
        /// <returns></returns>
        public string CreateToken(object data, int min = 60)
        {
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds) + min;

            var payload = new
            {
                exp = secondsSinceEpoch,
                data = data
            };
            
            return CreateToken(payload);
        }

        /// <summary>
        /// 解密token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public T DecodeToken<T>(string token)
        {
            try
            {
                //secret需要加密
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
                var json = decoder.DecodeToObject<T>(token, secret, verify: true);
                
                return json;
            }
            catch (TokenExpiredException)
            {
                throw new TokenExpiredException("Token已过期"); 
            }
            catch (SignatureVerificationException)
            {
                throw  new SignatureVerificationException("Token签名无效");
            }
        }
    }
}
