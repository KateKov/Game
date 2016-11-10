using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.EF;
using GameStore.DAL.Interfaces;
using NLog;

namespace GameStore.DAL.Infrastracture
{
    public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
    {
        private readonly GameStoreContext _db;

        private readonly ILogger _logger;

        protected GameStoreContext DbContext
        {
            get
            {
                try
                {
                    return _db ?? new GameStoreContext();
                }
                catch (EntityException ex)
                {
                    _logger.Error("The load DbContext failed: {0}", ex.StackTrace);
                    return new GameStoreContext();
                }          
            }
        }

        public Repository(GameStoreContext db)
        {
            _db = db;
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return DbContext.Set<T>();
            }
            catch (Exception ex)
            {
                _logger.Error("The try get all entities {0}  failed: {1}", (new T()).GetType(), ex.StackTrace);
                return new List<T>();
            }
        }

        public virtual IEnumerable<T> All
        {
            get
            {
                return GetAll();
            }
        }

        public virtual IEnumerable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            try
            {
                IQueryable<T> query = DbContext.Set<T>();
                foreach (var includeProperty in includeProperties)
                {
                    query = query.Include(includeProperty);
                }

                return query;
            }
            catch (Exception ex)
            {
                _logger.Error("The chosen of all file including properties failed entity {0}: {1}", (new T()).GetType(),  ex.StackTrace);
                return new List<T>();
            }
        }

        public T GetSingle(int? id)
        {
            try
            {
                return GetAll().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {               
                _logger.Error("There is no file Type {0} with Id {1}: {2} ", (new T()).GetType(), id, ex.StackTrace);
                return new T();
            }
        }

        public virtual IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {     
            return DbContext.Set<T>().Where(predicate);
        }

        public virtual void Add(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
                DbContext.Set<T>().Add(entity);
                _db.SaveChanges();
            }
            catch (EntityException ex)
            {
                _logger.Error("The added entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }
        }

        public virtual void Edit(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
                _db.SaveChanges();
            }
            catch (EntityException ex)
            {
                _logger.Error("The updating entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }
        }

        public virtual void Delete(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Deleted;
                _db.SaveChanges();
            }
            catch (EntityException ex)
            {
                _logger.Error("The deleted entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }
        }
    }
}
