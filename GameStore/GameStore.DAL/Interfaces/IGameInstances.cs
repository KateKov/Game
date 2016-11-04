using GameStore.DAL.Entities;

namespace GameStore.DAL.Interfaces
{
    public interface IGameInstances
    {
        string Key { get; set; }
        Game Game { get; set; }
    }
}
