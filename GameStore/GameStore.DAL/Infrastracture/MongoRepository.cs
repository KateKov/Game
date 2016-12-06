using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.DAL.Interfaces;
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

        public void Add(T entity)
        {
            _mongoConnectionHandler.MongoCollection.Save(entity,new MongoInsertOptions {  WriteConcern = WriteConcern.Acknowledged});
        }

        public void MakeOutdated(string id)
        {
            var mongo = GetSingle(id);
            mongo.IsOutdated = true;
            _logging.MongoCollection.Save(new Logging(DateTime.UtcNow, "Make outdated entity in mongodb", "Product/Game", "old version:" + GetVersion(mongo)));
            var res = MongoDB.Driver.Builders.Query<T>.EQ(pd => pd.MongoId, mongo.MongoId);
            var operation = Update<T>.Replace(mongo);
            _mongoConnectionHandler.MongoCollection.Update(res, operation);
        }

        public MongoRepository()
        {
            _mongoConnectionHandler = new MongoConnectionHandler<T>();
            _logging = new MongoConnectionHandler<Logging>();
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
                       typeof(T).Name, "old version:" + GetVersion(GetSingle(entity.Id)) + "new version:" + GetVersion(entity)));
            var res = MongoDB.Driver.Builders.Query<T>.EQ(pd => pd.MongoId, entity.MongoId);
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
            MakeOutdated(entity.Id);
            return false;
        }

        public IEnumerable<T> GetAll()
        {
             return _mongoConnectionHandler.MongoCollection.FindAll().Where(x=>x.IsOutdated.Equals(false));
        }

        public T GetSingle(string id)
        {
            if (id.Length == new ObjectId().ToString().Length)
            {
                var entityQuery = MongoDB.Driver.Builders.Query<T>.EQ(e => e.MongoId, new ObjectId(id));
                return _mongoConnectionHandler.MongoCollection.FindOne(entityQuery);
            }
            return null;
        }

        public IEnumerable<TD> FindBy<TD>(Expression<Func<TD, bool>> predicate) where TD: class,IEntityBase, new()
        {
            var entities = (new T() is OrderDetailMongo)? new List<T>() : _mongoConnectionHandler.MongoCollection.FindAll().ToList();
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

