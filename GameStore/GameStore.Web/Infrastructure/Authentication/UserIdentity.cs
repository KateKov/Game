using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace GameStore.Web.Infrastructure.Authentication
{
    public class UserIdentity : IIdentity
    {
        private readonly UserModel _user;

        public UserIdentity()
        {
            _user = new UserModel { Username = "guest", Roles = new List<UserRole> { UserRole.Guest } };
        }

        public UserIdentity(UserModel userData)
        {
            _user = userData;
        }

        public UserModel User
        {
            get { return _user; }
        }

        public virtual string Name
        {
            get { return User.Username; }
        }

        public virtual string AuthenticationType
        {
            get { return typeof(UserModel).Name; }
        }

        public virtual bool IsAuthenticated
        {
            get { return !User.Roles.Contains(UserRole.Guest); }
        }
    }
}