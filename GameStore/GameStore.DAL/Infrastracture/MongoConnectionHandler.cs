using GameStore.DAL.Interfaces;
using MongoDB.Driver;

namespace GameStore.DAL.Infrastracture
{
    public class MongoConnectionHandler<T> where T : IMongoEntity
    {
        public MongoCollection<T> MongoCollection { get; private set; }
        public MongoConnectionHandler()
        {
            var map = Map();
            if (map != null)
            {
                var mongoClient = new MongoClient("mongodb://localhost/test");
                var mongoServer = mongoClient.GetServer();
                var db = mongoServer.GetDatabase("Northwind");
                MongoCollection = db.GetCollection<T>(map);
            }
        }

        private string Map()
        {
            var name = typeof(T).Name;
            switch (name)
            {
                case "Supplier":
                    return "suppliers";
                case "Product":
                    return "products";
                case "Category":
                    return "categories";
                case "Shipper":
                    return "shippers";
                case "OrderDetailMongo":
                    return "order-details";
                case "OrderMongo":
                    return "orders";
                case "Logging":
                    return "logging";
                default:
                    return null;
            }
        }
    }
}
