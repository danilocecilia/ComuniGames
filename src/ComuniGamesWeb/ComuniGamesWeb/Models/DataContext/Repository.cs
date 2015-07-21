using System;
using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace ComuniGamesWeb.Models.DataContext
{
    public class Repository<T> : IRepository<T> where T : class
    {
        internal DbSet<T> DbSet;
        readonly DbContext _datacontext;

        public Repository(DbContext datacontext)
        {
            _datacontext = new ComuniGamesEntities();
            this.DbSet = _datacontext.Set<T>();
        }

        #region IRepository<T> Members

        public void Insert(T entity)
        {
            this.DbSet.Add(entity);
            Save();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IQueryable<T> GetAll()
        {
            return DbSet;
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).FirstOrDefault();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Dispose()
        {
           
        }

        private bool Save()
        {
            return this._datacontext.SaveChanges() > 0;
        }

        public void Update(T entity)
        {
            //var entry = this._datacontext.Entry<T>(entity);

            //var pkey = this.DbSet.Create().GetType().GetProperty("ID").GetValue(entity);

            //if (entry.State == EntityState.Detached)
            //{
            //    var set = _datacontext.Set<T>();
            //    T attachedEntity = set.Find(pkey);  // You need to have access to key

            //    if (attachedEntity != null)
            //    {
            //        var attachedEntry = _datacontext.Entry(attachedEntity);
            //        attachedEntry.CurrentValues.SetValues(entity);
            //    }
            //    else
            //    {
            //        entry.State = EntityState.Modified; // This should attach entity
            //    }
            //}
            //Save();
        }
        #endregion
    }
}