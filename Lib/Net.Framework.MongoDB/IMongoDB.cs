using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Net.Framework.MongoDB
{
    public interface IMongoDB
    {
        /// <summary>
        /// 日志的创建时间
        /// </summary>
        DateTime CreateTime { get; set; }
    }
}
