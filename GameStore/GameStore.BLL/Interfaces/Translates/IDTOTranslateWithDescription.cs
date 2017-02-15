namespace GameStore.BLL.Interfaces
{
    public interface IDTOTranslateWithDescription : IDTOTranslate
    {
        string Description { get; set; }
    }
}
