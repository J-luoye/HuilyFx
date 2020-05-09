using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SQLite;
using System.IO;
using DataLib.SqlServer;

namespace DataLib.SQLite
{
    /// <summary>
    /// sqlitedb helper
    /// </summary>
    public class SqliteContext : DbContext
    {
        /// <summary>
        /// db路径
        /// </summary>
        public static readonly string DbFile = ConfigurationManager.AppSettings["SqLiteDb"].ToString();

        /// <summary>
        /// 用户数据
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// sqlite数据库上下文
        /// </summary>
        public SqliteContext()
            : this(DbFile)
        {
        }

        /// <summary>
        /// sqlite数据库上下文
        /// </summary>
        /// <param name="dbFile">db文件路径</param>
        /// <param name="pooling">pool连接方式</param>
        public SqliteContext(string dbFile, bool pooling = true)
            : base(CreateConnection(dbFile, pooling), true)
        {
        }


        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="dbFile"></param>
        /// <param name="pooling">pool连接方式</param>
        /// <returns></returns>
        private static SQLiteConnection CreateConnection(string dbFile, bool pooling)
        {
            if (File.Exists(dbFile) == false)
            {
                throw new FileNotFoundException($"找不到db文件{dbFile}");
            }
            var constring = $"Data Source={dbFile};Pooling={pooling}";
            return new SQLiteConnection(constring);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 备份数据库
        /// </summary>
        public static string BackDb()
        {
            if (File.Exists(DbFile) == false)
            {
                throw new FileNotFoundException($"找不到db文件{DbFile}");
            }

            var fileName = Path.GetFileNameWithoutExtension(DbFile);
            var fileExt = Path.GetExtension(DbFile);
            var dir = @"backDb\\";
            var destFile = dir + fileName + DateTime.Now.ToLongDateString() + fileExt;
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            File.Copy(Path.GetFullPath(DbFile), Path.GetFullPath(destFile), true);
            return Path.GetFullPath(destFile);
        }

        ///// <summary>
        ///// 更新
        ///// </summary>
        ///// <typeparam name="T">类型</typeparam>
        ///// <param name="model">模型</param>
        ///// <param name="id">id</param>
        ///// <param name="newModel">更新后的模型</param>
        ///// <returns></returns>
        //private DbEntityEntry<T> Update<T>(T model, object id, out T newModel) where T : class
        //{
        //    try
        //    {
        //        var entry = this.Entry(model);
        //        if (entry.State == EntityState.Detached)
        //        {
        //            this.Set<T>().Attach(model);
        //            entry.State = EntityState.Modified;
        //        }
        //        newModel = model;
        //        return entry;
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        newModel = this.Set<T>().Find(id);
        //        var entry = this.Entry(newModel);
        //        entry.CurrentValues.SetValues(model);
        //        return entry;
        //    }
        //}

        ///// <summary>
        ///// 更新实体
        ///// </summary>
        ///// <param name="model">实体</param>
        //public T Update<T>(T model, object id) where T : class
        //{
        //    T newModel;
        //    this.Update<T>(model, id, out newModel);
        //    return newModel;
        //}

        ///// <summary>
        ///// 局部更新
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TPartial"></typeparam>
        ///// <param name="model">模型</param>
        ///// <param name="id">id</param>
        ///// <param name="partial">更新的字段</param>
        ///// <returns></returns>
        //public T UpdatePartial<T, TPartial>(T model, object id, Expression<Func<T, TPartial>> partial)
        //    where T : class
        //    where TPartial : class
        //{
        //    var value = partial.Compile().Invoke(model);
        //    var newModel = this.Set<T>().Find(id);
        //    var entry = this.Entry(newModel);
        //    entry.CurrentValues.SetValues(value);
        //    return newModel;
        //}

        ///// <summary>
        ///// 排除更新
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="TExcept"></typeparam>
        ///// <param name="model">模型</param>
        ///// <param name="id">id</param>
        ///// <param name="except">排除的字段</param>
        ///// <returns></returns>
        //public T UpdateExcept<T, TExcept>(T model, object id, Expression<Func<T, TExcept>> except)
        //    where T : class
        //    where TExcept : class
        //{
        //    if (except == null)
        //    {
        //        throw new ArgumentNullException("except");
        //    }

        //    var newExp = except.Body as NewExpression;
        //    if (newExp == null)
        //    {
        //        throw new InvalidCastException();
        //    }

        //    T newModel;
        //    var entry = this.Update(model, id, out newModel);
        //    foreach (var m in newExp.Members)
        //    {
        //        var property = entry.Property(m.Name);
        //        property.CurrentValue = property.OriginalValue;
        //        property.IsModified = false;
        //    }
        //    return newModel;
        //}
    }
}
