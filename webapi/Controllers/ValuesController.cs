using webapi.Enum;
using webapi.Models;

namespace webapi.Controllers
{
    /// <summary>
    /// 业务接口
    /// </summary>
    public class ValuesController : ApiBaseController
    {
        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <returns></returns>
        public ResultModel Get()
        {
            AuthInfo info = this.Model;
            if (info == null)
            {
                return new ResultModel{ code = ResultCode.Error, Message = "失败"};
            }
            else
            {
                return new ResultModel { code = ResultCode.Success, Message = "成功", Data = info};
            }
        }
    }
}
