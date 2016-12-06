using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    public class MongoBase : IMongoEntity
    {
        [BsonId]
        public ObjectId MongoId { get; set; }
        public bool IsOutdated { get; set; }
        public string Id { get { return MongoId.ToString(); } }

    }
}
