using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;

namespace GameStore.DAL.Enums
{
    public enum NotificationMethod
    {
        [Display(Name = "Mail", ResourceType = typeof(GlobalRes))]
        Mail,
        [Display(Name = "Sms", ResourceType = typeof(GlobalRes))]
        Sms,
        [Display(Name = "Mobile", ResourceType = typeof(GlobalRes))]
        Mobail
    };
}
