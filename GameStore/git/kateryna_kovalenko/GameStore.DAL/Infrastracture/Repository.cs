using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Data.Entity.Core;
using GameStore.DAL.Interfaces;
using GameStore.DAL.EF;
using NLog;
using NLog.Fluent;

namespace GameStore.DAL.Infrastracture
{
    public class Repository<T> : IRepository<T> where T:class, IEntityBase, new()
    {
        private GameStoreContext db;

        private ILogger logger;

        protected GameStoreContext DbContext
        {
            get
            {
                try
                {
                    return db ?? (db = new GameStoreContext());
                }
                catch(EntityException ex)
                {
                    logger.Error("The load DbContext failed: {0}", ex.StackTrace);
                    return new GameStoreContext();
                }
                
            }
        }
        public Repository(GameStoreContext db, ILogger logger)
        {
            this.logger = logger;
            this.db = db;
        }

        public virtual IEnumerable<T> GetAll()
        {
            try
            {
                return DbContext.Set<T>();
            }
            catch (Exception ex)
            {
                logger.Error("The try get all entities {0}  failed: {1}", (new T()).GetType(), ex.StackTrace);
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
            catch(Exception ex)
            {
                logger.Error("The chosen of all file including properties failed entity {0}: {1}", (new T()).GetType(),  ex.StackTrace);
                return new List<T>();
            }
        }
        public T GetSingle(int id)
        {
            try
            {
                return GetAll().FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                
                logger.Error("There is no file Type {0} with Id {1}: {2} ", (new T()).GetType(), id, ex.StackTrace);
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
                db.SaveChanges();
            }
            catch (EntityException ex)
            {
                logger.Error("The added entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }
        }
        public virtual void Edit(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (EntityException ex)
            {
                logger.Error("The updating entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }

        }
        public virtual void Delete(T entity)
        {
            try
            {
                DbEntityEntry dbEntityEntry = DbContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (EntityException ex)
            {
                logger.Error("The deleted entity {0} with Id {1} failed: {2}", entity.GetType(), entity.Id, ex.StackTrace);
            }
        }

      
    }
}
