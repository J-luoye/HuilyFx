using JwtDotnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace webapi.Models
{
    /// <summary>
    /// 验证token过滤器
    /// </summary>
    public class ApiAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.TryGetValues("Authorization", out IEnumerable<string> tokens);
            if (tokens != null)
            {
                string token = tokens.FirstOrDefault().Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var json = new TokenManager(authKey.secret).DecodeToken<AuthInfo>(token);
                        if (json != null)
                        {
                            actionContext.RequestContext.RouteData.Values.Add("Authorization", json);
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}