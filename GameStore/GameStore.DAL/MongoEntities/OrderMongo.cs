using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class OrderMongo : MongoBase
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public bool IsConfirmed { get; set; }
        public decimal Sum { get; set; }
    }
}
