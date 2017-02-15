using GameStore.BLL.Interfaces;

namespace GameStore.BLL.Infrastructure
{
    public class DtoToEntity<T> where T : class, IDtoBase, new()
    {
        public static bool IsEntity()
        {
            var name = typeof(T).Name;
            return (name.Contains("Comment") || name.Contains("Genre") || name.Contains("Game") || name.Contains("Role") || name.Contains("User") ||
                    name.Contains("OrderDetail") || name.Contains("PlatformType") || name.Contains("Publisher") ||
                    name.Contains("Order"));
        }
    }
}