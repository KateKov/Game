using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class Shipper : MongoBase
    {
        public string CompanyName { get; set; }
        public string Phone { get; set; }
    }
}
