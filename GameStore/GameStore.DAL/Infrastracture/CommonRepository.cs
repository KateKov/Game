using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.DAL.EF;
using GameStore.DAL.Entities;
using GameStore.DAL.MongoEntities;
using GameStore.DAL.Mapping;

namespace GameStore.DAL.Infrastracture
{
    public class CommonRepository<T, TD> : IRepository<T> where T : class, IEntityBase,  new() where TD: class, IMongoEntity, new()
    {
        private readonly IMongoRepository<Logging> _logging;
        private readonly IRepository<T> _sqlRepository;
        private readonly IMongoEntity _mongoType;
        private readonly IMongoRepository<TD> _mongoRepository;
        private readonly GameStoreContext _context;
      
        public CommonRepository(GameStoreContext db)
        {
            _mongoType = GetMongo<T>.GetMongoType();
            _sqlRepository = new SQLRepository<T>(db);
            _logging = new MongoRepository<Logging>();
            _mongoRepository = new MongoRepository<TD>();
            _context = db;
        }

        public void Add(T entity)
        {
            if (entity is Game)
            {
                if (_mongoRepository.GetSingle((entity as Game).Key)!=null)
                {
                    _mongoRepository.MakeOutdated((entity as Game).Key);
                } 
            }
            _sqlRepository.Add(entity);
            _logging.Add(new Logging(DateTime.UtcNow, "Add entity from Sql", typeof(T).Name, MongoRepository<TD>.GetVersion(entity)));
        }

        public void Delete(T entity)
        {
            if (entity.EntityId == Guid.Empty)
            {
                _mongoRepository.MakeOutdated(entity.Id);
            }
            else
            {
                _sqlRepository.Delete(entity);
            }
            _logging.Add(new Logging(DateTime.UtcNow, "Delete entity from Sql", typeof(T).Name, MongoRepository<TD>.GetVersion(entity)));
        }

        public void Edit(T entity)
        {
            var model = entity;
            if (GetMongo<T>.IsMongo() && !_sqlRepository.FindBy(x=>x.Id == entity.Id).Any())
            {
                model.EntityId = Guid.NewGuid();
                _logging.Add(new Logging(DateTime.UtcNow, "Editing entities in mongo", typeof(T).Name, "old version:" + MongoRepository<TD>.GetVersion(GetSingle(model.Id)) + " ; new version: " + MongoRepository<TD>.GetVersion(entity)));
                if (!_mongoRepository.Edit(Mapper.Map<TD>(entity)))
                {
                    _sqlRepository.Add(model);
                }
            }
            else
            {
                _logging.Add(new Logging(DateTime.UtcNow, "Editing entity in sql", typeof(T).Name, "old version:" + MongoRepository<TD>.GetVersion(GetSingle(model.EntityId.ToString())) + " ; new version: " + MongoRepository<TD>.GetVersion(entity)));
                _sqlRepository.Edit(model);
            }
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var entities = _sqlRepository.FindBy(predicate).ToList();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.FindBy(predicate);
                entities.AddRange(mongo);
            }
            return entities;
        }

        private QueryResult<T> GetQuery(Query<T> query, IEnumerable<T> entities)
        {
            var models = entities.AsQueryable();
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

         private IEnumerable<T> GetFunc(Func<T, bool> where, IEnumerable<T> models)
        {
            return models.Where(where).ToList();
        }

        public IEnumerable<T> GetAll()
        {
            var entities = _sqlRepository.GetAll().ToList();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.GetAll();
                entities.AddRange(Mapper.Map<IEnumerable<T>>(mongo));
            }
            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities", typeof(T).Name, ""));
            return entities;
        }

        public QueryResult<T> GetAll(Query<T> query)
        {
            var sql = _sqlRepository.GetAll(query);
            IEnumerable<T> entities =new List<T>();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.GetAll().AsQueryable();
                IEnumerable<T> mongoToSql = new List<T>();
                foreach (var item in mongo)
                {
                    var entity = new MongoToSql().GetEntityFromMongo<T, TD>(item);                 
                    IEnumerable<T> listEntity = (entity!=null) ?new List<T>() {entity} : new List<T>();
                    mongoToSql = mongoToSql.Union(listEntity);
                }
                entities = entities.Union(mongoToSql);
            }
            var mongoQuery = GetQuery(query, entities);
            sql.List = sql.List.Union(mongoQuery.List);
            sql.Count += mongoQuery.Count;
            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities by query: "+query, typeof(T).Name, ""));
            return sql;
        }

        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            var sql =_sqlRepository.GetAll(where).ToList();
            IEnumerable<T> entities = new List<T>();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.GetAll().AsQueryable();
                IEnumerable<T> mongoToSql = new List<T>();
                foreach (var item in mongo)
                {
                    var entity = new MongoToSql().GetEntityFromMongo<T, TD>(item);
                    IEnumerable<T> listEntity = (entity != null) ? new List<T>() { entity } : new List<T>();
                    mongoToSql = mongoToSql.AsQueryable().Union(listEntity);
                }
                entities = entities.Union(mongoToSql);
            }
            var mongoFuc = GetFunc(where, entities);
            sql.AddRange(mongoFuc);
            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities by function: " + where, typeof(T).Name, ""));
            return sql;
        }

        public T GetSingle(string id)
        {
            if (id.Length == Guid.Empty.ToString().Length)
            {
                var entity = _sqlRepository.GetSingle(id);
                if (entity != null)
                {
                    _logging.Add(new Logging(DateTime.UtcNow, "Get single entity from sql", typeof(T).Name,
                        MongoRepository<TD>.GetVersion(entity)));
                    return entity;
                }
            }
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.GetSingle(id);
                return Mapper.Map<T>(mongo);
            }
            return null;
        }
    }
}
