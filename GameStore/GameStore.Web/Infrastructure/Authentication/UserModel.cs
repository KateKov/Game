using System.Collections.Generic;

namespace GameStore.Web.Infrastructure.Authentication
{
    public class UserModel
    {
        public string Username { get; set; }
        public IEnumerable<UserRole> Roles { get; set; } 
        public bool IsBanned { get; set; }
    }
}