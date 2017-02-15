using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Mapping;
using GameStore.DAL.MongoEntities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;


namespace GameStore.DAL.Infrastracture
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class, IMongoEntity, new()
    {
        private readonly MongoConnectionHandler<T> _mongoConnectionHandler;
        private readonly MongoConnectionHandler<Logging> _logging;

        public MongoRepository()
        {
            _mongoConnectionHandler = new MongoConnectionHandler<T>();
            _logging = new MongoConnectionHandler<Logging>();
        }

        public void Add(T entity)
        {
            _mongoConnectionHandler.MongoCollection.Save(entity,
                new MongoInsertOptions {WriteConcern = WriteConcern.Acknowledged});
        }

        public void MakeOutdated(string id)
        {
            var mongo = GetSingle(id);
            if (mongo == null)
            {
                return;
            }

            mongo.IsOutdated = true;
            _logging.MongoCollection.Save(new Logging(DateTime.UtcNow, "Make outdated entity in mongodb", "Product/Game",
                "old version:" + GetVersion(mongo)));
            EditMongo(mongo);
        }

        public void MakeDeleted(string id)
        {
            var mongo = GetSingle(id);
            if (mongo == null)
            {
                return;
            }

            mongo.IsDeleted = true;
            _logging.MongoCollection.Save(new Logging(DateTime.UtcNow, "Make deleted entity in mongodb", "Product/Game",
                "old version:" + GetVersion(mongo)));
            Edit(mongo);
        }

        public static string GetVersion<TD>(TD entity)
        {
            var version = "";
            if (entity != null)
            {
                typeof(TD)
                    .GetProperties()
                    .Select(x => x.GetValue(entity, null))
                    .ToList()
                    .ForEach(x => version += x);
            }

            return version;
        }

        private void EditMongo(T entity)
        {
            _logging.MongoCollection.Save(new Logging(DateTime.UtcNow, "Edit mongo entity in mongodb",
                typeof(T).Name, "old version:" + GetVersion(GetSingle(entity.Id.ToString())) + "new version:" + GetVersion(entity)));
            var res = MongoDB.Driver.Builders.Query<T>.EQ(pd => pd.Id, entity.Id);
            var operation = Update<T>.Replace(entity);
            _mongoConnectionHandler.MongoCollection.Update(res, operation);
        }

        public bool Edit(T entity)
        {
            if ((entity is Category && (entity as Category).CategoryID != 0) ||
                (entity is Supplier && (entity as Supplier).SupplierID != 0))
            {
                EditMongo(entity);
                return true;
            }

            MakeOutdated(entity.Id.ToString());
            return false;
        }

        public IEnumerable<T> GetAll()
        {
            return _mongoConnectionHandler.MongoCollection.FindAll().Where(x => x.IsOutdated.Equals(false));
        }

        public T GetSingle(string id)
        {
            if (id.Length != 32 || id.Length!=24)
            {
                return null;
            }

            var key = (id.Length == 24) ? new ObjectId(id) : Guid.Parse(id).AsObjectId();
            var entityQuery = MongoDB.Driver.Builders.Query<T>.EQ(e => e.Id, key);
            return (entityQuery !=null && _mongoConnectionHandler.MongoCollection != null) ? _mongoConnectionHandler.MongoCollection.FindOne(entityQuery) : null;
        }

        public IEnumerable<TD> FindBy<TD>(Expression<Func<TD, bool>> predicate) where TD : class, IEntityBase, new()
        {
            var entities = (new T() is OrderDetailMongo)
                ? new List<T>()
                : _mongoConnectionHandler.MongoCollection.FindAll().ToList();
            var map = Mapper.Map<IEnumerable<T>, IEnumerable<TD>>(entities);
            return map.AsQueryable().Where(predicate);
        }

        public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = MongoDB.Driver.Builders.Query<T>.Where(predicate);
            return _mongoConnectionHandler.MongoCollection.Find(query);
        }
    }
}