using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "UserName")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
           ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        public string Username { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(GlobalRes),
          ErrorMessageResourceName = "RequiredError")]
        [StringLength(50)]
        public string Email { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "IsLocked")]
        public bool IsLocked { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "FilterGame_DateOfAdding")]
        public DateTime CreateDate { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "IsDeleted")]
        public bool IsDeleted { get; set; }


        public string PublisherId { get; set; }

        public NotificationMethod Method { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Publisher")]
        public string Publisher { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Orders")]
        public virtual ICollection<string> Orders { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "Roles")]
        public virtual ICollection<string> Roles { get; set; }



        [Display(ResourceType = typeof(GlobalRes), Name = "Comments")]
        public virtual ICollection<string> Comments { get; set; }


        [Display(ResourceType = typeof(GlobalRes), Name = "Bans")]
        public virtual ICollection<string> Bans { get; set; }
    }
}