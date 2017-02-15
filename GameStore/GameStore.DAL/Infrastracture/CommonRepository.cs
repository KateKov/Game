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
    public class CommonRepository<T, TD> : IRepository<T> where T : class, IEntityBase, new()
        where TD : class, IMongoEntity, new()
    {
        private readonly IMongoRepository<Logging> _logging;
        private readonly IRepository<T> _sqlRepository;
        private readonly IMongoRepository<TD> _mongoRepository;

        public CommonRepository(GameStoreContext db)
        {
            _sqlRepository = new SQLRepository<T>(db);
            _logging = new MongoRepository<Logging>();
            _mongoRepository = new MongoRepository<TD>();
        }

        public void Add(T entity)
        {
            if (GetMongo<T>.IsMongo())
            {
                if (entity is Game && _mongoRepository.GetSingle((entity as Game).Key) != null)
                {
                    _mongoRepository.MakeOutdated((entity as Game).Key);
                }
                else if (_mongoRepository.GetSingle(entity.Id.AsObjectId().ToString()) != null)
                {
                    _mongoRepository.MakeOutdated(entity.Id.AsObjectId().ToString());
                }
            }

            _sqlRepository.Add(entity);
            _logging.Add(new Logging(DateTime.UtcNow, "Add entity from Sql", typeof(T).Name,
                MongoRepository<TD>.GetVersion(entity)));
        }

        public int GetTotalNumber(bool isWithDeleted)
        {
            var mongoCount = (isWithDeleted)
                ? _mongoRepository.GetAll().Count()
                : _mongoRepository.GetAll().Count(x => !x.IsDeleted);
            var count = _sqlRepository.GetTotalNumber(isWithDeleted) + mongoCount;
            return count;
        }

        public bool IsExist(string id)
        {
            var mongo = _mongoRepository.GetSingle(id) != null;
            return _sqlRepository.IsExist(id) || mongo;
        }

        public int GetCount(Func<T, bool> where, bool isWithDeleted)
        {
            var sqlCount = _sqlRepository.GetCount(where, isWithDeleted);
            if (GetMongo<T>.IsMongo())
            {
                var mongo = (isWithDeleted)
                    ? _mongoRepository.GetAll().AsQueryable()
                    : _mongoRepository.GetAll().Where(x => !x.IsDeleted).AsQueryable();
                var mongoCount = 0;
                foreach (var item in mongo)
                {
                    var mongoEntity = new MongoToSql().GetEntityFromMongo<T, TD>(item);
                    mongoCount += (mongoEntity != null && where(mongoEntity)) ? 1 : 0;
                }
                sqlCount += mongoCount;
            }
            return sqlCount;
        }

        public void Delete(T entity)
        {
            var model = entity;
            model.IsDeleted = true;
            if (!_sqlRepository.IsExist(entity.Id.ToString()))
            {
                _mongoRepository.MakeDeleted(entity.Id.AsObjectId().ToString());
                _sqlRepository.Add(model);
            }
            else
            {
                _sqlRepository.Edit(entity);
            }

            _logging.Add(new Logging(DateTime.UtcNow, "Delete entity from Sql", typeof(T).Name,
                MongoRepository<TD>.GetVersion(entity)));
        }

        public void Edit(T entity)
        {
            var model = entity;
            if (GetMongo<T>.IsMongo() &&
                _sqlRepository.FindBy(x => x.Id == entity.Id).FirstOrDefault() == null)
            {
                _logging.Add(new Logging(DateTime.UtcNow, "Editing entities in mongo", typeof(T).Name,
                    "old version:" + MongoRepository<TD>.GetVersion(GetSingle(model.Id.AsObjectId().ToString())) + " ; new version: " +
                    MongoRepository<TD>.GetVersion(entity)));
                _mongoRepository.MakeOutdated(entity.Id.AsObjectId().ToString());
                _sqlRepository.Add(model);
            }
            else
            {
                _logging.Add(new Logging(DateTime.UtcNow, "Editing entity in sql", typeof(T).Name,
                    "old version:" + MongoRepository<TD>.GetVersion(GetSingle(model.Id.ToString())) +
                    " ; new version: " + MongoRepository<TD>.GetVersion(entity)));
                _sqlRepository.Edit(model);
            }
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var entities = _sqlRepository.FindBy(predicate).ToList();
            if (GetMongo<T>.IsMongo()&&!(new T() is Order) && !(new T() is OrderDetail))
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
            if (GetMongo<T>.IsMongo() && !(new T() is Order) && !(new T() is OrderDetail))
            {
                var mongo = _mongoRepository.GetAll();
                entities.AddRange(Mapper.Map<IEnumerable<T>>(mongo));
            }

            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities", typeof(T).Name, ""));
            return entities;
        }

        public QueryResult<T> GetAll(Query<T> query, bool isWithDeleted = false)
        {
            var sql = _sqlRepository.GetAll(query, isWithDeleted);
            IEnumerable<T> entities = new List<T>();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = (isWithDeleted)
                    ? _mongoRepository.GetAll().AsQueryable()
                    : _mongoRepository.GetAll().Where(x => !x.IsDeleted).AsQueryable();
                IEnumerable<T> mongoToSql = new List<T>();
                foreach (var item in mongo)
                {
                    var entity = new MongoToSql().GetEntityFromMongo<T, TD>(item);
                    IEnumerable<T> listEntity = (entity != null) ? new List<T>() {entity} : new List<T>();
                    mongoToSql = mongoToSql.Union(listEntity);
                }
                entities = entities.Union(mongoToSql);
            }

            var mongoQuery = GetQuery(query, entities);
            sql.List = sql.List.Union(mongoQuery.List);
            sql.Count += mongoQuery.Count;
            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities by query: " + query, typeof(T).Name, ""));

            return sql;
        }

        public IEnumerable<T> GetAll(Func<T, bool> where)
        {
            var sql = _sqlRepository.GetAll(where).ToList();
            IEnumerable<T> entities = new List<T>();
            if (GetMongo<T>.IsMongo())
            {
                var mongo = _mongoRepository.GetAll().AsQueryable();
                IEnumerable<T> mongoToSql = new List<T>();
                foreach (var item in mongo)
                {
                    var entity = new MongoToSql().GetEntityFromMongo<T, TD>(item);
                    IEnumerable<T> listEntity = (entity != null) ? new List<T>() {entity} : new List<T>();
                    mongoToSql = mongoToSql.AsQueryable().Union(listEntity);
                }

                entities = entities.Union(mongoToSql);
            }

            var mongoFuc = (entities.Any())?GetFunc(where, entities):entities;
            sql.AddRange(mongoFuc);
            _logging.Add(new Logging(DateTime.UtcNow, "Get all entities by function", typeof(T).Name, ""));

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