using Common.Page;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLib.SqlServer
{
    /// <summary>
    /// 分页信息
    /// </summary>
    public static class PageInfoEx
    {
        /// <summary>
        /// 执行分页        
        /// 性能比较好
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>    
        /// <param name="orderBy">排序字符串</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static async Task<PageInfo<T>> ToPageAsync<T>(this IQueryable<T> source, string orderBy, int pageIndex, int pageSize) where T : class
        {
            int total = await source.CountAsync();
            var inc = total % pageSize > 0 ? 0 : -1;
            var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
            pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

            var data = await source.OrderBy(orderBy).Skip(pageIndex * pageSize).Take(pageSize).AsNoTracking().ToListAsync();
            var page = new PageInfo<T>(total, data) { PageIndex = pageIndex, PageSize = pageSize };
            return page;
        }

        /// <summary>
        /// 执行分页        
        /// 性能比较好
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>    
        /// <param name="orderBy">排序字符串</param>
        /// <param name="pageIndex">分页索引</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static async Task<PageInfo<T>> ToPageAsync<T, TIdKey>(this IQueryable<T> source, string orderBy, int pageIndex, int pageSize, Expression<Func<T, TIdKey>> idKeySelecotr) where T : class
        {
            int total = await source.CountAsync();
            var inc = total % pageSize > 0 ? 0 : -1;
            var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
            pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

            var orderBySource = source.OrderBy(orderBy);
            var ids = await orderBySource.Skip(pageIndex * pageSize).Take(pageSize).Select(idKeySelecotr).ToArrayAsync();
            if (ids.Length <= 0)
            {
                return new PageInfo<T>(total, new T[0]) { PageIndex = pageIndex, PageSize = pageSize };
            }
            var inWhere = ConverToOrExpressions<T, TIdKey>(idKeySelecotr, ids);
            var datas = await orderBySource.Where(inWhere).ToArrayAsync();
            var page = new PageInfo<T>(total, datas) { PageIndex = pageIndex, PageSize = pageSize };
            return page;
        }


        /// <summary>
        /// 将数组转换为Or的表示式合集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="keySelector">键选择</param>
        /// <param name="values">包含的值</param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> ConverToOrExpressions<T, TKey>(Expression<Func<T, TKey>> keySelector, IEnumerable<TKey> values)
        {
            var p = keySelector.Parameters.Single();
            var equals = values.Select(value => (Expression)Expression.Equal(keySelector.Body, Expression.Constant(value, typeof(TKey))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }
    }
}
