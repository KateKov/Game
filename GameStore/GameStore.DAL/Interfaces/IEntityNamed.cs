namespace GameStore.DAL.Interfaces
{
    public interface IEntityNamed: IEntityBase
    {
        string Name { get; set; }
    }
}
