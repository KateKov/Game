using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class OrderDetailMongo : MongoBase
    {
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public double Discount { get; set; }
        public int OrderID { get; set; }     
        public int ProductId { get; set; }
    }
}
