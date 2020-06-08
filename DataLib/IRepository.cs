﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataLib
{
    /// <summary>
    /// Just to mark a class as repository.
    /// </summary>
    public interface IRepository
    {
    }

    public interface IRepository<TEntity> : IRepository<TEntity, long> where TEntity : class, IEntity
    {
    }

    public interface IRepository<TEntity, TKey> : IRepository
    {
        void Add(TEntity entity);

        void AddRange(IList<TEntity> entities);

        void Remove(TEntity entity);

        IDbContextTransaction BeginTransaction();

        Task<int> SaveChangesAsync();

        IQueryable<TEntity> Query();
    }
}
