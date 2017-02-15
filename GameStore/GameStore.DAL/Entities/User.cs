using System;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class User : EntityBase
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Ban> Bans { get; set; }
        public virtual ManagerProfile ManagerProfile { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
