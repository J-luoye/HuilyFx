﻿using DataLib.SqlServer;
using PredicateLib;
using System;
using System.Data.Entity;

namespace HuilyFx
{

    /// <summary>
    /// 自己重载需要使用的dbset
    /// </summary>
    public abstract class DBSET : SqlDb
    {
        public DbSet<User> user { get; set; }

    }

    public class User
    {
        public string Id { get; set; }
        public int? Age { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }


    public class Class1
    {
        /// <summary>
        /// 查询条件测试案列
        /// </summary>
        public void PredicateTest()
        {
            var uri = new Uri("http://www.xx.com/?age=1&name=%E8%80%81%E4%B9%9D&id=001");

            var condition = uri.AsCondition<User>()
                .OperatorFor(item => item.Age, Operator.GreaterThan)
                .IgnoreFor(item => item.Id);

            var predicate = condition
                .ToAndPredicate()
                .Or(item => item.Birthday < DateTime.Now);

            Console.WriteLine(predicate);
            Console.ReadLine();
        }
    }
}
