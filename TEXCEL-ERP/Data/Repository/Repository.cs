using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace TEXCEL_ERP.Data.Repository
{
    /// <summary>
    /// Generic repository implementation.
    /// </summary>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T GetById(long id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.FirstOrDefault(predicate);
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).ToList();
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Count(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Any(predicate);
        }
    }
}