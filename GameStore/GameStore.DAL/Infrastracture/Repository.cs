using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.EF;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Infrastracture
{
    public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
    {
        private readonly GameStoreContext _db;

        public Repository(GameStoreContext db)
        {
            _db = db;
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return _db.Set<T>();
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }

        public T GetSingle(Guid id)
        {
            try
            {
                return GetAll().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception)
            {               
                return new T();
            }
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {     
            return _db.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            _db.Entry(entity);
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry dbEntityEntry = _db.Entry(entity);
            dbEntityEntry.State = EntityState.Modified;
            _db.SaveChanges();
        }

        public virtual void Delete(T entity)
        {
            DbEntityEntry dbEntityEntry = _db.Entry(entity);
            dbEntityEntry.State = EntityState.Deleted;
            _db.SaveChanges();
        }
    }
}
