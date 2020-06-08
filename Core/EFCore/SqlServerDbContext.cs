using System;
using System.Collections.Generic;
using System.Text;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class SqlServerDbContext : DbContext
    {
        /// <summary>
        /// 测试用户
        /// </summary>
        public DbSet<User> AdManage { get; set; }

        public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
            : base(options)
        {

        }
    }
}
