using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using webapi.Cache;
using webapi.Controllers;
using webapi.Enum;

namespace webapi.Models
{
    /// <summary>
    /// 后台登录
    /// </summary>
    public class LoginAuthInfoAttribute : AuthorizationFilterAttribute
    {
        /// <summary>
        /// 授权管理
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.TryGetValues("Authorization", out IEnumerable<string> tokens);
            if (tokens == null || tokens.Count() <= 0)
            {
                SetTokenInvalidResponse(actionContext, ResultCode.TokenInvalid);
                return;
            }
            var token = tokens.FirstOrDefault().Replace("Bearer ", "");

            var model = CacheClient.GetModel<AuthInfo>(token);
            if (model == null)
            {
                SetTokenInvalidResponse(actionContext, ResultCode.TokenInvalid);
                return;
            }
            var controller = actionContext.ControllerContext.Controller as ApiBaseController;
            if (controller != null)
            {
                controller.Model = model;
            }
        }

        /// <summary>
        /// 设置token无效结果
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="code"></param>
        private void SetTokenInvalidResponse(HttpActionContext actionContext, ResultCode code)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.OK,new ResultModel(){ code  = code, Message = code.GetFieldDisplay() });
        }
    }
}