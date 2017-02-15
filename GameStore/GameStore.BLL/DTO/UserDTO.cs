using System;
using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.DAL.Enums;

namespace GameStore.BLL.DTO
{
    public class UserDTO : IDtoBase
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsDeleted { get; set; }
        public NotificationMethod Method { get; set; }

        public virtual ICollection<string> Orders { get; set; }
        public virtual ICollection<UserDTOTranslate> Translates { get; set; }

        public virtual ICollection<string> Roles { get; set; }
        public virtual ICollection<string> Comments { get; set; }
        public virtual ICollection<string> Bans { get; set; }
    }
}
