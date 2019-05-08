using System.Web.Http;
using webapi.Models;

namespace webapi.Controllers
{
    /// <summary>
    /// 基础API控制器
    /// </summary>
    [ApiAuthorize]
    [LoginAuthInfo]
    public class ApiBaseController : ApiController
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public AuthInfo Model { get; set; }

        /// <summary>
        /// 用户登录token
        /// </summary>
        public string Token { get; set; }
    }
}
