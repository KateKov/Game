using MongoDB.Bson;

namespace GameStore.DAL.Interfaces
{
    public interface IMongoEntity
    {
        ObjectId MongoId { get; set; }
        bool IsOutdated { get; set; }
        string Id { get; }
    }
}
