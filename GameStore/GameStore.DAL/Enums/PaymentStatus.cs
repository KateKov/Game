using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;

namespace GameStore.DAL.Enums
{
    public enum PaymentStatus
    {
        [Display(Name = "Succesful", ResourceType = typeof(GlobalRes))]
        Succesful,

        [Display(Name = "NotEnoughMoney", ResourceType = typeof(GlobalRes))]
        NotEnoughMoney,

        [Display(Name = "DoesNotExist", ResourceType = typeof(GlobalRes))]
        CardDoesnotExist,

        [Display(Name = "Failed", ResourceType = typeof(GlobalRes))]
        Failed
    }
}
