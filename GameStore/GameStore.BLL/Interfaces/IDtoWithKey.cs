namespace GameStore.BLL.Interfaces
{
    public interface IDtoWithKey : IDtoBase
    {
       string Key { get; set; }
    }
}
