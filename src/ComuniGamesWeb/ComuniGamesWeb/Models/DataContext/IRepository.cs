using System;
using System.Linq;
using System.Linq.Expressions;

namespace ComuniGamesWeb.Models.DataContext
{
    public interface IRepository<T> : IDisposable
    {
        void Update(T entity);
        void Insert(T entity);
        void Delete(T entity);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        T GetById(int id);
        T Get(Expression<Func<T, bool>> predicate);
    }
}