using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.MongoEntities
{
    [BsonIgnoreExtraElements]
    public class Supplier : MongoBase
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string HomePage { get; set; }
        public List<Product> Products { get; set; }

        public Supplier()
        {
            Products = new List<Product>();
        }
    }
}
