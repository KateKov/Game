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
    public class SQLRepository<T> : IRepository<T> where T : class, IEntityBase, new()
    {
        private readonly GameStoreContext _db;

        public SQLRepository(GameStoreContext db)
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

        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            return GetAll().Where(where).ToList();
        }

        public int GetTotalNumber(bool isWithDeleted)
        {
            return (isWithDeleted)?_db.Set<T>().Count() : _db.Set<T>().Count(x=>!x.IsDeleted);
        }

        public bool IsExist(string id)
        {
            return _db.Set<T>().Any(x => x.Id.ToString() == id);
        }

        public int GetCount(Func<T, bool> where, bool isWithDeleted = false)
        {
            return (isWithDeleted) ? _db.Set<T>().Count(where) : _db.Set<T>().Where(x => !x.IsDeleted).Count(where);
        }

        public QueryResult<T> GetAll(Query<T> query, bool isWithDeleted = false)
        {
            var models = (isWithDeleted) ? GetAll().AsQueryable() : GetAll().Where(x => !x.IsDeleted).AsQueryable();
            if (query.Where != null)
            {
                models = models.Where(query.Where).AsQueryable();
            }

            if (query.OrderBy != null)
            {
                models = models.OrderBy(query.OrderBy).AsQueryable();
            }

            var count = models.Count();
            if (query.Skip != null)
            {
                models = models.Skip(query.Skip.Value);
            }

            if (query.Take != null)
            {
                models = models.Take(query.Take.Value);
            }

            var queryResult = new QueryResult<T>
            {
                List = models,
                Count = count
            };

            return queryResult;
        }

        public T GetSingle(string id)
        {
            try
            {
                return GetAll().FirstOrDefault(x => x.Id.ToString() == id);
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
            _db.Set<T>().Add(entity);
            _db.SaveChanges();
        }

        public void Edit(T entity)
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

        public void Delete(string id)
        {
            var entity = GetSingle(id);
            Delete(entity);
        }
    }
}
