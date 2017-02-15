using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class Product : MongoBase
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }


        public string Key
        {
            get { return Id.ToString(); }
            set { }
        }
        public decimal UnitPrice { get; set; }
        public bool Discountinued { get; set; }
        public short UnitsOnOrder { get; set; }
        public short UnitsInStock { get; set; }

        public int CategoryID { get; set; }

        public int SupplierID { get; set; }
    }
}
