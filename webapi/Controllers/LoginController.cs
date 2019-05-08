using JwtDotnet;
using System;
using System.Linq;
using System.Web.Http;
using webapi.Cache;
using webapi.Enum;
using webapi.Models;

namespace webapi.Controllers
{
    /// <summary>
    /// 登录中心
    /// </summary>
    public class LoginController : ApiController
    {
        [System.Web.Http.HttpPost()]
        public ResultModel Login([FromBody]LoginRequest request)
        {
            ResultModel rs = new ResultModel();
            //这是是获取用户名和密码的，这里只是为了模拟
            if ((request.UserName == "lzj" || request.UserName != string.Empty) && (request.Password == "123456" || request.Password == string.Empty))
            {
                //模拟用户数据
                AuthInfo info = Datas.AuthInfos().Where(item => item.UserName == request.UserName).FirstOrDefault();
                try
                {
                    var token = new TokenManager(authKey.secret).CreateToken(info, 100);
                    CacheClient.CacheInsert(token, info, DateTime.MaxValue);
                    rs.Message = string.Empty;
                    rs.Data = token;
                    rs.code = ResultCode.Success;
                }
                catch (Exception ex)
                {
                    rs.Message = ex.Message;
                    rs.code = ResultCode.Error;
                }
            }
            else
            {
                rs.Message = "fail";
                rs.code = ResultCode.Error;
            }
            return rs;
        }
    }
}
