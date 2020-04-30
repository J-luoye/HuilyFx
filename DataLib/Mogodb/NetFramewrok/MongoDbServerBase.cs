using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLib.Mogodb.NetFramewrok
{
    /// <summary>
    /// 服务基础类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public abstract class MongoDbServerBase<T> where T : class, IMongoDB
    //{
    //    /// <summary>
    //    /// 当前月
    //    /// </summary>
    //    private readonly DateTime month;

    //    /// <summary>
    //    /// 连接字符串
    //    /// </summary>
    //    private static readonly string Con = ConfigurationManager.ConnectionStrings["Mongodb"].ConnectionString;

    //    /// <summary>
    //    /// 客户端
    //    /// </summary>
    //    private static readonly MongoClient client = new MongoClient(Con);


    //    /// <summary>
    //    /// 获取动态数据库名
    //    /// </summary>
    //    protected string DbName
    //    {
    //        get
    //        {
    //            return "RKELog" + this.month.ToString("yyMM");
    //        }
    //    }


    //    /// <summary>
    //    /// 日志服务基础类
    //    /// </summary>
    //    public MongoDbServerBase()
    //        : this(DateTime.Today)
    //    {
    //    }

    //    /// <summary>
    //    /// 日志服务基础类
    //    /// </summary>
    //    /// <param name="month">选择月</param>
    //    public MongoDbServerBase(DateTime? month)
    //    {
    //        if (month == null)
    //        {
    //            this.month = DateTime.Today;
    //        }
    //        else
    //        {
    //            this.month = month.Value;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取数据库对象
    //    /// </summary>
    //    /// <returns></returns>
    //    protected IMongoDatabase GetDataBase()
    //    {
    //        return client.GetDatabase(this.DbName);
    //    }

    //    /// <summary>
    //    /// 返回集合对象
    //    /// </summary>
    //    /// <returns></returns>
    //    private IMongoCollection<T> GetCollection()
    //    {
    //        var coName = typeof(T).Name;
    //        return this.GetDataBase().GetCollection<T>(coName);
    //    }

    //    /// <summary>
    //    /// 清除所有记录
    //    /// </summary>
    //    public void Clear()
    //    {
    //        this.GetDataBase().DropCollection(typeof(T).Name);
    //    }

    //    /// <summary>
    //    /// 添加数据
    //    /// </summary>
    //    /// <param name="model">模型</param>
    //    public virtual void Add(T model)
    //    {
    //        this.GetCollection().InsertOne(model);
    //    }

    //    /// <summary>
    //    /// 添加数据
    //    /// </summary>
    //    /// <param name="model">模型</param>
    //    public virtual async Task AddAsync(T model)
    //    {
    //        await this.GetCollection().InsertOneAsync(model);
    //    }

    //    /// <summary>
    //    /// 添加数据
    //    /// </summary>
    //    /// <param name="model">模型</param>
    //    public virtual async Task AddAsync(IEnumerable<T> model)
    //    {
    //        await this.GetCollection().InsertManyAsync(model);
    //    }

    //    /// <summary>
    //    /// 查找符合条件第一条记录
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <returns></returns>
    //    public async Task<T> FindAsync(Expression<Func<T, bool>> where)
    //    {
    //        var cursor = await this.GetCollection().FindAsync(where);
    //        return await cursor.FirstOrDefaultAsync();
    //    }

    //    /// <summary>
    //    /// 查找符合条件第一条记录
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <returns></returns>
    //    public T Find(Expression<Func<T, bool>> where)
    //    {
    //        return this.GetCollection().AsQueryable().Where(where).FirstOrDefault();
    //    }

    //    /// <summary>
    //    /// 查找符合条件第一条记录
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <param name="orderBy">排序</param>
    //    /// <returns></returns>
    //    public T Find<TKey>(Expression<Func<T, bool>> where, Expression<Func<T, TKey>> orderBy)
    //    {
    //        return this.GetCollection().AsQueryable().Where(where).OrderBy(orderBy).FirstOrDefault();
    //    }

    //    /// <summary>
    //    /// 查找符合条件的记录
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <returns></returns>
    //    public T[] FindMany(Expression<Func<T, bool>> where)
    //    {
    //        return this.GetCollection().AsQueryable().Where(where).OrderByDescending(item => item.CreateTime).ToArray();
    //    }

    //    /// <summary>
    //    /// 条件更新
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <param name="updater">更新</param>
    //    /// <returns></returns>
    //    public async Task<int> UpdateAsync(Expression<Func<T, bool>> where, UpdateDefinition<T> updater)
    //    {
    //        var result = await this.GetCollection().UpdateManyAsync(where, updater);
    //        return (int)result.ModifiedCount;
    //    }

    //    /// <summary>
    //    /// 获取分页
    //    /// </summary>
    //    /// <param name="where">条件</param>
    //    /// <param name="pageIndex">页面索引</param>
    //    /// <param name="pageSize">页面大小</param>
    //    /// <returns></returns>
    //    public virtual PageInfo<T> GetPage(Expression<Func<T, bool>> where, int pageIndex, int pageSize)
    //    {
    //        //查询优化，除操作日志没有社区ID,其余日志都加上社区ID 索引
    //        OnCommuityIDEnsureIndex();

    //        var index = this.OnEnsureIndex(new IndexKeysDefinitionBuilder<T>());
    //        this.GetCollection().Indexes.CreateOne(index);
    //        try
    //        {
    //            return this.GetCollection().ToPage(where, pageIndex, pageSize);
    //        }
    //        catch (FormatException)
    //        {
    //            this.Clear();
    //            return new PageInfo<T>(0, Enumerable.Empty<T>()) { PageIndex = pageIndex, PageSize = pageSize };
    //        }
    //    }

    //    /// <summary>
    //    /// 添加社区ID索引
    //    /// </summary>
    //    /// <returns></returns>
    //    protected virtual void OnCommuityIDEnsureIndex()
    //    {
    //        //查询优化，除操作日志没有社区ID,其余日志都加上社区ID 索引
    //        if (typeof(T) != typeof(OperateLog))
    //        {
    //            var coidIndex = new IndexKeysDefinitionBuilder<T>().Descending("CommunityID");
    //            this.GetCollection().Indexes.CreateOne(coidIndex);
    //        }
    //    }

    //    /// <summary>
    //    /// 建立索引
    //    /// </summary>
    //    /// <param name="index">索引器</param>
    //    /// <returns></returns>
    //    protected virtual IndexKeysDefinition<T> OnEnsureIndex(IndexKeysDefinitionBuilder<T> index)
    //    {
    //        return index.Descending(item => item.CreateTime);
    //    }


    //    /// <summary>
    //    /// 获取当前月往前连接的几个月
    //    /// </summary>
    //    /// <param name="m">共几个月</param>
    //    /// <returns></returns>
    //    public IEnumerable<DateTime> GetMonths(int m)
    //    {
    //        var current = DateTime.Today.AddDays(1 - DateTime.Today.Day);
    //        for (var i = 0; i < m; i++)
    //        {
    //            yield return current.AddMonths(-i);
    //        }
    //    }
    //}
}
