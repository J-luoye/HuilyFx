using Common.Page;
using EntityFramework.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLib.SqlServer
{
    /// <summary>
    /// Sql存储服务基础类
    /// </summary>
    public abstract class SqlServerBase
    {
        /// <summary>
        /// 获取数据库上下文
        /// </summary>
        public SqlDb Db { get; set; }

        /// <summary>
        /// 保存变更
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SaveChangesAsync()
        {
            return (await this.Db.SaveChangesAsync()) > 0;
        }


        /// <summary>
        /// 当调用SaveChanges之后执行
        /// </summary>
        /// <param name="asyncAction">委托</param>
        public void AfterSaveChanges(Func<Task> asyncAction)
        {
            this.Db.AfterSaveChanges(asyncAction);
        }
    }


    /// <summary>
    /// 服务基础类
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    public abstract class ServerBaseT<T> : SqlServerBase, IDisposable where T : class
    {
        /// <summary>
        /// 当前的表
        /// </summary>
        protected IDbSet<T> DbSet
        {
            get
            {
                return this.Db.Set<T>();
            }
        }

        /// <summary>
        /// 根据id查询记录
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public T Find(params object[] id)
        {
            return DbSet.Find(id);
        }

        /// <summary>
        /// 根据条件查询第一条记录
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> where)
        {
            return await this.DbSet.Where(where).FirstOrDefaultAsync<T>();
        }

        /// <summary>
        /// 根据条件查询第一条记录
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <param name="where">查询条件</param>
        /// <param name="selector">选择字段</param>
        /// <returns></returns>
        public async Task<TNew> FindAsync<TNew>(Expression<Func<T, bool>> where, Expression<Func<T, TNew>> selector)
        {
            return await this.DbSet.Where(where).Select(selector).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 根据条件查询返回对象集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public virtual async Task<T[]> FindManyAsync(Expression<Func<T, bool>> where)
        {
            return await this.DbSet.Where(where).ToArrayAsync();
        }

        /// <summary>
        /// 根据条件查询返回对象集合
        /// </summary>
        /// <typeparam name="TNew"></typeparam>
        /// <param name="where">条件</param>
        /// <param name="selector">映射</param>
        /// <returns></returns>
        public virtual async Task<TNew[]> FindManyAsync<TNew>(Expression<Func<T, bool>> where, Expression<Func<T, TNew>> selector)
        {
            return await this.DbSet.Where(where).Select(selector).ToArrayAsync();
        }

        /// <summary>
        /// 根据条件查询返回对象集合
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="orderBy">排序</param>
        /// <returns></returns>
        public virtual async Task<T[]> FindManyAsync(Expression<Func<T, bool>> where, string orderBy)
        {
            return await this.DbSet.Where(where).OrderBy(orderBy).ToArrayAsync();
        }

        /// <summary>
        /// 将数组转换为Or的表示式合集
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <typeparam name="TKey">键类型</typeparam>
        /// <param name="keySelector">键选择</param>
        /// <param name="values">包含的值</param>
        /// <returns></returns>
        private Expression<Func<T, bool>> ConverToOrExpressions<TKey>(Expression<Func<T, TKey>> keySelector, IEnumerable<TKey> values)
        {
            var p = keySelector.Parameters.Single();
            var equals = values.Select(value => (Expression)Expression.Equal(keySelector.Body, Expression.Constant(value, typeof(TKey))));
            var body = equals.Aggregate<Expression>((accumulate, equal) => Expression.Or(accumulate, equal));
            return Expression.Lambda<Func<T, bool>>(body, p);
        }


        /// <summary>
        /// 根据条件去查询分页数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页码 从零开始</param>
        /// <param name="pageSize">每页条数</param>
        /// <param name="idKeySelecotr">id字段选择表达式</param>
        /// <returns></returns>
        public async Task<PageInfo<T>> GetPagesAsync<TIdKey>(Expression<Func<T, bool>> where, string orderBy, int pageIndex, int pageSize, Expression<Func<T, TIdKey>> idKeySelecotr)
        {
            var source = this.DbSet.Where(where);
            int total = source.Count();
            var inc = total % pageSize > 0 ? 0 : -1;
            var maxPageIndex = (int)Math.Floor((double)total / pageSize) + inc;
            pageIndex = Math.Max(0, Math.Min(pageIndex, maxPageIndex));

            var orderBySource = source.OrderBy(orderBy);
            var ids = await orderBySource.Skip(pageIndex * pageSize).Take(pageSize).Select(idKeySelecotr).ToArrayAsync();
            if (ids.Length <= 0)
            {
                return new PageInfo<T>(total, new T[0]) { PageIndex = pageIndex, PageSize = pageSize };
            }
            var inWhere = this.ConverToOrExpressions<TIdKey>(idKeySelecotr, ids);
            var datas = orderBySource.Where(inWhere);

            var page = new PageInfo<T>(total, datas) { PageIndex = pageIndex, PageSize = pageSize };
            return page;
        }

        /// <summary>
        /// 根据条件去查询分页数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="orderBy">排序</param>
        /// <param name="pageIndex">当前页码 从零开始</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public async Task<PageInfo<T>> GetPagesAsync(Expression<Func<T, bool>> where, string orderBy, int pageIndex, int pageSize)
        {
            return await this.DbSet.Where(where).ToPageAsync(orderBy, pageIndex, pageSize);
        }

        /// <summary>
        /// 获取记录条数
        /// </summary>
        /// <returns></returns>
        public async Task<int> CountAsync()
        {
            return await this.DbSet.CountAsync();
        }

        /// <summary>
        /// 根据条件查询记录条数
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await this.DbSet.CountAsync(where);
        }

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> where)
        {
            return await this.DbSet.AnyAsync(where);
        }

        /// <summary>
        /// 释放连接上下文
        /// </summary>
        public void Dispose()
        {
            this.Db.Dispose();
        }
    }


    /// <summary>
    /// 服务基础类
    /// </summary>
    /// <typeparam name="T">模型类型</typeparam>
    public abstract class ServerBase<T> : ServerBaseT<T> where T : class
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public virtual T Add(T model)
        {
            return this.DbSet.Add(model);
        }

        /// <summary>
        /// 删除 
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public virtual T Remove(object id)
        {
            var model = this.DbSet.Find(id);
            if (model != null)
            {
                this.DbSet.Remove(model);
            }
            return model;
        }

        /// <summary>
        /// EF扩展
        /// 条件删除，直接生效
        /// 与其它更新或删除方法一起使用时需要添加事务
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public virtual int Remove(Expression<Func<T, bool>> where)
        {
            return this.DbSet.Where(where).Delete();
        }

        /// <summary>
        /// EF扩展
        /// 条件更新，直接生效
        /// 与其它更新或删除方法一起使用时需要添加事务
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="update">更新表达式</param>
        /// <returns></returns>
        public int Update(Expression<Func<T, bool>> where, Expression<Func<T, T>> update)
        {
            return this.DbSet.Where(where).Update(update);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="id">模型id</param>
        /// <returns></returns>
        public virtual T Update(T model, object id)
        {
            return this.Db.Update(model, id);
        }
    }
}
