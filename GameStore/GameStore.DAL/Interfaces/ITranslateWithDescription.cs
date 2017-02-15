namespace GameStore.DAL.Interfaces
{
    public interface ITranslateWithDescription : ITranslate
    {
        string Description { get; set; }      
    }
}
