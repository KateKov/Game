using GameStore.BLL.DTO;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces.Services
{
    public interface IUserService : IService<UserDTO>
    {
        UserDTO GetUserByName(string username);

        UserDTO Login(string username, string password);

        bool IsExistByUsername(string username);

        bool IsExistByEmail(string email);

        bool IsInRole(string username, string rolename);

        bool IsBanned(string username);

        User GetUserEntityByName(string username);
        void ChangeNotificationMethod(string username, NotificationMethod method);

        void Ban(string username, string reason, Duration duration);

        void Unban(string username);

        void Register(UserDTO userDto);

        void Edit(UserDTO userDto);

    }
}
