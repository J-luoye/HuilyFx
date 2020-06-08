using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Core
{
    /// <summary>
    /// 获取DbContext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbContextService<T> : ScopeApplicationService where T : class
    {
        private readonly SqlServerDbContext _Context;

        public DbContextService(SqlServerDbContext sqliteContext)
        {
            this._Context = sqliteContext;
        }

        /// <summary>
        /// 获取Context
        /// </summary>
        public DbSet<T> Context => _Context.Set<T>();

        public async Task<int> SaveChangesAsync()
        {
            return await _Context.SaveChangesAsync();
        }

        public void Entry(T entity, EntityState entityState)
        {
            _Context.Entry(entity).State = entityState;
        }

        /// <summary>
        /// 事务
        /// </summary>
        /// <returns></returns>
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _Context.Database.BeginTransactionAsync();
        }

        /// <summary>
        /// 获取单个数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<T> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _Context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync(Expression<Func<T, bool>> predicate)
        {
            return (await _Context.Set<T>().FirstOrDefaultAsync(predicate)) == null;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            _Context.Set<T>().Add(entity);
            return entity;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        public void RemoveAsync(T entity)
        {
            _Context.Set<T>().Remove(entity);
        }
    }
}
