using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ADYC.IRepository
{
    public interface IReadOnlyRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Get(int id);
        IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        // This method was not in the videos, but I thought it would be useful to add.
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> filter = null);
    }
}
