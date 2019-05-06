using System.Collections.Generic;

namespace webapi.Models
{
    /// <summary>
    /// 模拟数据
    /// </summary>
    public class Datas
    {
        public static List<AuthInfo> AuthInfos()
        {
            return new List<AuthInfo>{
                new AuthInfo() {  UserName = "admin", Roles = new List<string>{ "admin","manager" }, IsAdmin = true},
                new AuthInfo() {  UserName = "user01", Roles = new List<string>{ "user" }, IsAdmin = false},
                new AuthInfo() {  UserName = "user02", Roles = new List<string>{ "user" }, IsAdmin = false},
                new AuthInfo() {  UserName = "user03", Roles = new List<string>{ "user" }, IsAdmin = false},
                new AuthInfo() {  UserName = "user04", Roles = new List<string>{ "user" }, IsAdmin = false},
                new AuthInfo() {  UserName = "lzj", Roles = new List<string>{ "admin","manager" }, IsAdmin = true},
            };
        }
    }
}