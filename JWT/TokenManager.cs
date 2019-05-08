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
        /// 创建token
        /// </summary>
        /// <param name="payload">自定义数据</param>
        /// <param name="secret">密匙</param>
        /// <returns></returns>
        public string CreateToken(object data,int month = 100)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            
            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow();

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // or use JwtValidator.UnixEpoch
            var secondsSinceEpoch = Math.Round((now - unixEpoch).TotalSeconds);

            var payload = new Dictionary<string, object>
            {
                { "name", "jwt"},
                {"exp",secondsSinceEpoch+month },
                {"jti",data }
            };


            var token = encoder.Encode(payload, secret);
            return token;
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
            catch (TokenExpiredException tokenEx)
            {
                throw new TokenExpiredException("Token has expired"); 
            }
            catch (SignatureVerificationException svEx)
            {
                throw  new SignatureVerificationException("Token has invalid signature");
            }
        }

    }
}
