using DataLib.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace DataLib.SQLite
{
    public class SqliteContextCore : DbContext
    {
        /// <summary>
        /// 用户DB
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        /// <param name="options"></param>
        public SqliteContextCore(DbContextOptions<SqliteContextCore> options)
            : base(options)
        {
        }
    }
}
