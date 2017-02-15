using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;

namespace GameStore.DAL.Enums
{
    public enum PageEnum
    {

       [Display(Name = "Ten", ResourceType = typeof(GlobalRes))]
        Ten = 10,
        [Display(Name = "Twenty", ResourceType = typeof(GlobalRes))]
        Twenty = 20,
        [Display(Name = "Fifty", ResourceType = typeof(GlobalRes))]
        Fifty = 50,
        [Display(Name = "Hundred", ResourceType = typeof(GlobalRes))]
        OneHundred = 100,
        [Display(Name = "All", ResourceType = typeof(GlobalRes))]
        All = 0,
    }
}
