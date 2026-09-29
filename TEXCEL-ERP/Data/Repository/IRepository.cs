using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace TEXCEL_ERP.Data.Repository
{
    /// <summary>
    /// Generic repository interface.
    /// </summary>
    public interface IRepository<T> where T : class
    {
        T GetById(int id);
        T GetById(long id);
        T Get(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> Query();
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        int Count();
        int Count(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
    }
}