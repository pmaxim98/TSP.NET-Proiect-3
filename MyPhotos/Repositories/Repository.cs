using MyPhotos.Model;
using MyPhotos.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace MyPhotos.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<T> entities;

        public Repository(MyPhotosContainer context)
        {
            this.context = context;
            entities = this.context.Set<T>();
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return entities.Any(predicate);
        }

        public void Add(T entity)
        {
            entities.Add(entity);
        }

        public void Remove(T entity)
        {
            entities.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> enumerable)
        {
            entities.RemoveRange(enumerable);
        }

        public void Update(T entity)
        {
            entities.Attach(entity);
            var entry = context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public void Update(T item, T initialEntity)
        {
            context.Entry(initialEntity).CurrentValues.SetValues(item);
        }

        public IQueryable<T> Include<TEntity>(Expression<Func<T, TEntity>> predicate)
        {
            return entities.Include(predicate);
        }

        public IEnumerable<T> Search(Expression<Func<T, bool>> predicate)
        {
            return entities.Where(predicate);
        }

        public T GetById(int id)
        {
            return entities.Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return entities.ToList();
        }
    }
}
