using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;

namespace GameStore.DAL.Enums
{
    public enum Duration
    {
        [Display(Name = "Hour", ResourceType = typeof(GlobalRes))]
        Hour,
        [Display(Name = "Day", ResourceType = typeof(GlobalRes))]
        Day,
        [Display(Name = "Week", ResourceType = typeof(GlobalRes))]
        Week,
        [Display(Name = "Month", ResourceType = typeof(GlobalRes))]
        Month,
        [Display(Name = "Permanent", ResourceType = typeof(GlobalRes))]
        Permanent
    };
}
