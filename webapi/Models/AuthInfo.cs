using System.Collections.Generic;

namespace webapi.Models
{
    public class AuthInfo
    {
        //模拟JWT的payload
        public string UserName { get; set; }

        public List<string> Roles { get; set; }

        public bool IsAdmin { get; set; }
    }
}