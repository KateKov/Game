using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDTO : IDtoBase
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
