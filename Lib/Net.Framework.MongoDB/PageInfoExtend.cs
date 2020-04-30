using System;
using System.Linq;
using System.Linq.Expressions;
using Common.Page;
using MongoDB.Driver;

namespace Net.Framework.MongoDB
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public static class PageInfoExtend
    {

        /// <summary>
        /// 执行分页            
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>    
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="orderBy">排序字符串</param>
        /// <returns></returns>
        public static PageInfo<T> ToPage<T>(this IQueryable<T> source, int pageIndex, int pageSize, string orderBy) where T : class
        {
            int total = source.Count();

            var inc = total % pageSize > 0 ? 0 : -1;
            var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
            pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

            var data = source.OrderBy(orderBy)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize)
                             .AsQueryable()
                             .ToList();
            var page = new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
            return page;
        }


        /// <summary>
        /// 根据时间倒序排序分页,控制数量
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="where">条件</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static PageInfo<T> ToPage<T>(this IMongoCollection<T> source, Expression<Func<T, bool>> where, int pageIndex, int pageSize) where T : class, IMongoDB
        {
            int total = source.AsQueryable().Count(where);

            var data = source
               .Aggregate(new AggregateOptions { AllowDiskUse = true })
               .Match(where)
               .SortByDescending(item => item.CreateTime)
               .Skip(pageIndex * pageSize)
               .Limit(pageSize)
               .ToList();

            return new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
        }
    }
}
