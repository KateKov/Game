namespace GameStore.DAL.Interfaces
{
    public interface IEntityWithKey : IEntityBase
    {
        string Key { get; set; }
    }
}
