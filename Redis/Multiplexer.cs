using StackExchange.Redis;
using System;
using System.Collections.Generic;

namespace Redis
{
    /// <summary>
    /// 表示Redis连接对象
    /// </summary>
    public class Multiplexer
    {
        /// <summary>
        /// 同步锁
        /// </summary>
        private static readonly object syncRoot = new object();

        /// <summary>
        /// 连接管理
        /// </summary>
        private static readonly Dictionary<string, ConnectionMultiplexer> table = new Dictionary<string, ConnectionMultiplexer>(StringComparer.OrdinalIgnoreCase);


        /// <summary>
        /// 获取连接名称
        /// </summary>
        public string Name { get; private set; }


        /// <summary>
        /// Redis连接对象
        /// </summary>
        /// <param name="name">连接名称</param>
        public Multiplexer(string name)
        {
            this.Name = name;
        }


        /// <summary>
        /// 获取连接对象
        /// </summary> 
        /// <returns></returns>
        public ConnectionMultiplexer GetMultiplexer()
        {
            lock (syncRoot)
            {
                var muliplexer = default(ConnectionMultiplexer);
                if (table.TryGetValue(this.Name, out muliplexer) == false)
                {
                    muliplexer = Multiplexer.CreateConnection(this.Name);
                    table[this.Name] = muliplexer;
                    return muliplexer;
                }

                if (muliplexer.IsConnected == true)
                {
                    return muliplexer;
                }
                else
                {
                    muliplexer.Dispose();
                }

                muliplexer = Multiplexer.CreateConnection(this.Name);
                table[this.Name] = muliplexer;
                return muliplexer;
            }
        }


        /// <summary>
        /// 创建一个连接
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static ConnectionMultiplexer CreateConnection(string name)
        {
            var config = ConfigurationManager.ConnectionStrings[name].ConnectionString;
            var opt = ConfigurationOptions.Parse(config);
            return ConnectionMultiplexer.Connect(opt);
        }
    }
}
