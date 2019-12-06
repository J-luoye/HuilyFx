using System;
using System.Web.Http;
using webapi.Enum;
using webapi.Models;

namespace webapi.Controllers
{
    /// <summary>
    /// 业务接口
    /// </summary>
    [RoutePrefix("api/values")]
    public class ValuesController : ApiBaseController
    { 
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserInfo")]
        public ResultModel GetUserInfo()
        {
            AuthInfo info = this.UserInfo;
            var Code = info == null ? ResultCode.Error : ResultCode.Success;
            return ResultModel.Custom(Code, info, Code.GetFieldDisplay());
        }

        public ResultModel GetRoles()
        {
            return ResultModel.Success();
        }

    }
}
