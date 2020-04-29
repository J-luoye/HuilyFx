using EntityFramework.Extensions;
using InterFaceManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataLib.SqlServer
{

    public class User
    {
        public int MyProperty { get; set; }

        public int MyProperty2 { get; set; }
    }

    /// <summary>
    /// Sql数据库
    /// </summary>
    public class SqlDb : DbContext
    {
        /// <summary>
        /// SaveChange之后执行的委托
        /// </summary>
        private readonly List<Func<Task>> asyncActionList = new List<Func<Task>>();

        public DbSet<User> User { get; set; }

        /// <summary>
        /// Sql数据库
        /// </summary>
        public SqlDb()
        {
            this.Database.CommandTimeout = 60 * 1000;
            this.Configuration.UseDatabaseNullSemantics = true;
        }

        /// <summary>
        /// 获取服务实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Server<T>() where T : SqlServerBase, new()
        {
            var server = new T();
            server.Db = this;
            return server;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="model">模型</param>
        /// <param name="id">id</param>
        /// <param name="newModel">更新后的模型</param>
        /// <returns></returns>
        public T Update<T>(T model) where T : class, IStringId
        {
            try
            {
                var entry = this.Entry(model);
                if (entry.State == EntityState.Detached)
                {
                    model = this.Set<T>().Attach(model);
                    entry.State = EntityState.Modified;
                }
                return model;
            }
            catch (InvalidOperationException)
            {
                var local = this.Set<T>().Find(model.Id);
                var entry = this.Entry(local);
                entry.CurrentValues.SetValues(model);
                return local;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="model">模型</param>
        /// <param name="id">id</param>
        /// <param name="newModel">更新后的模型</param>
        /// <returns></returns>
        private DbEntityEntry<T> Update<T>(T model, object id, out T newModel) where T : class
        {
            try
            {
                var entry = this.Entry(model);
                if (entry.State == EntityState.Detached)
                {
                    this.Set<T>().Attach(model);
                    entry.State = EntityState.Modified;
                }
                newModel = model;
                return entry;
            }
            catch (InvalidOperationException)
            {
                newModel = this.Set<T>().Find(id);
                var entry = this.Entry(newModel);
                entry.CurrentValues.SetValues(model);
                return entry;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="model">实体</param>
        public T Update<T>(T model, object id) where T : class
        {
            T newModel;
            this.Update<T>(model, id, out newModel);
            return newModel;
        }

        /// <summary>
        /// EF扩展
        /// 条件更新，直接生效
        /// 与其它更新或删除方法一起使用时需要添加事务
        /// </summary>
        /// <param name="where">条件</param>
        /// <param name="update">更新表达式</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> update) where T : class
        {
            return await this.Set<T>().Where(where).UpdateAsync(update);
        }

        /// <summary>
        /// 创建模型时
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 保存变化
        /// </summary>
        /// <returns></returns>
        public override async Task<int> SaveChangesAsync()
        {
            var count = await base.SaveChangesAsync(); 
            foreach (var asyncAction in this.asyncActionList)
            {
                await asyncAction();
            }
            this.asyncActionList.Clear();
            return count;
        }

        /// <summary>
        /// 保存变化
        /// </summary>
        /// <returns></returns>
        [Obsolete("请使用SaveChangesAsync", true)]
        public override int SaveChanges()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当调用SaveChanges之后执行
        /// </summary>
        /// <param name="asyncAction">委托</param>
        public void AfterSaveChanges(Func<Task> asyncAction)
        {
            this.asyncActionList.Add(asyncAction);
        }
    }
}
