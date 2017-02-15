using MongoDB.Bson;

namespace GameStore.DAL.Interfaces
{
    public interface IMongoEntity
    {
        ObjectId Id { get; set; }
        bool IsOutdated { get; set; }
        bool IsDeleted { get; set; }
    }
}
