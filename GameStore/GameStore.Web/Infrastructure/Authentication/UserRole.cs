using System;

namespace GameStore.Web.Infrastructure.Authentication
{
    [Flags]
    public enum UserRole 
    {
        Guest = 0,
        User = 1,
        Manager = 2,
        Moderator = 4,
        Administrator = 8
    }
}