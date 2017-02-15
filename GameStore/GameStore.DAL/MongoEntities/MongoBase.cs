using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    public class MongoBase : IMongoEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public bool IsOutdated { get; set; }

        public bool IsDeleted { get; set; }
    }
}
