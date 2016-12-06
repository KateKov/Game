using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
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
            get { return MongoId.ToString(); }
            set { }
        }
        public decimal UnitPrice { get; set; }
        public bool Discountinued { get; set; }
        public short UnitsOnOrder { get; set; }
        public short UnitsInStock { get; set; }

        //[BsonRepresentation(BsonType.String)]
        public int CategoryID { get; set; }

        //[BsonRepresentation(BsonType.String)]
        public int SupplierID { get; set; }
    }
}
