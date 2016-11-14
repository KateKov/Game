using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PlatformTypeDTO : IDtoBase, IDtoNamed
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
