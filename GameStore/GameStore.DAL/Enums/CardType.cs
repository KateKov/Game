using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;

namespace GameStore.DAL.Enums
{
    public enum CardType
    {
        [Display(Name = "Visa", ResourceType = typeof(GlobalRes))]
        Visa,
        [Display(Name = "MasterCard", ResourceType = typeof(GlobalRes))]
        MasterCard
    }
}
