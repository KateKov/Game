using System.Web;

namespace GameStore.Web.Interfaces
{
    public interface IAuthenticationManager 
    {
        HttpContext HttpContext { get; set; }

        IUserProvider CurrentUser { get; }

        bool Login(string login, string password, bool isPersistent);

        void LogOut();
    }
}