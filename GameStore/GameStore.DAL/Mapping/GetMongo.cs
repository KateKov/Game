using System;
using System.Collections.Generic;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;

namespace GameStore.DAL.Mapping
{
    public class GetMongo<T> where T: class, IEntityBase, new()
    {
        private static readonly Dictionary<MappingEntity, IMongoEntity> _mapping;
        private static readonly IMongoEntity _mongoType;
        private enum MappingEntity
        {
            Game,
            Genre,
            Publisher,
            OrderDetail,
            Order
        };

        static GetMongo()
        {
            _mapping = new Dictionary<MappingEntity, IMongoEntity>()
            {
                {MappingEntity.Game, new Product()},
                {MappingEntity.Genre, new Category()},
                {MappingEntity.Publisher, new Supplier()},
                 {MappingEntity.OrderDetail, new OrderDetailMongo()},
                {MappingEntity.Order, new OrderMongo() }
            };
            if (IsMongo())
            {
                var entityEnum = Enum.Parse(typeof(MappingEntity), typeof(T).Name);
                _mongoType = _mapping[(MappingEntity) entityEnum];
            }
        }

        public static bool IsMongo()
        {
            var name = typeof(T).Name;
            return (name.Equals("Game") || name.Equals("Genre") || name.Equals("Publisher") ||
                    name.Contains("Order") ||
                    name.Contains("OrderDetail"));
        }

        public static IMongoEntity GetMongoType()
        {
            return (IsMongo()) ? _mongoType : null;
        }
    }
}
