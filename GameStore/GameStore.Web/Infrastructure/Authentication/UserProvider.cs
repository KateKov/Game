using System;
using System.Collections.Generic;
using System.Linq;
using GameStore.Web.Interfaces;
using System.Security.Principal;

namespace GameStore.Web.Infrastructure.Authentication
{
    public class UserProvider : IUserProvider
    {
        private readonly UserIdentity _userIdentity;

        public UserProvider(UserModel user)
        {
            _userIdentity = new UserIdentity(user);
        }

        public UserProvider()
        {
            _userIdentity = new UserIdentity();
        }

        public UserModel Current
        {
            get { return _userIdentity.User; }
        }

        public bool IsInRole(UserRole userRole)
        {
            string[] roles = userRole.ToString().Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries); 
            return roles.Any(x => _userIdentity.User.Roles.Any(y => y.ToString() == x));
        }

        public bool IsBanned()
        {
            return Current.IsBanned;
        }

        public bool IsInRole(string role)
        {
            IEnumerable<UserRole> roles = _userIdentity.User.Roles;
            return roles.Any(r => string.Equals(r.ToString(), role, StringComparison.CurrentCultureIgnoreCase));
        }

        public IIdentity Identity
        {
            get { return _userIdentity; }
        }
    }
}