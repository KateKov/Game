using System.Security.Principal;
using GameStore.Web.Infrastructure.Authentication;

namespace GameStore.Web.Interfaces
{
    public interface IUserProvider : IPrincipal
    {
        UserModel Current { get; }

        bool IsInRole(UserRole userRole);

        bool IsBanned();
    }
}
