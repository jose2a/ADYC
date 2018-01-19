using ADYC.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;

namespace ADYC.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext context;
        protected DbSet<TEntity> Entity;

        public Repository(DbContext context)
        {
            this.context = context;
            Entity = context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            return Entity.Find(id);
        }

        public IEnumerable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = Entity;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public IEnumerable<TEntity> Find(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = Entity;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> filter = null)
        {
            return Entity.SingleOrDefault(filter);
        }

        public void Add(TEntity entity)
        {
            try
            {
                Entity.Add(entity);

                Save();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            Entity.AddRange(entities);

            Save();
        }

        public void Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                Entity.Attach(entity);
            }

            Entity.Remove(entity);

            Save();
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Entity.RemoveRange(entities);

            Save();
        }

        public void Update(TEntity entity)
        {
            Entity.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
