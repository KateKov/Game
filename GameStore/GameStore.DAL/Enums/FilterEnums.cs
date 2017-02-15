using System.ComponentModel.DataAnnotations;
using GameStore.DAL.App_LocalResources;


namespace GameStore.DAL.Enums
{
    public enum Date
    {
        [Display(Name = "LastWeek", ResourceType = typeof(GlobalRes))]
        week,
        [Display(Name = "LasMonth", ResourceType = typeof(GlobalRes))]
        month,
        [Display(Name = "LastYear", ResourceType = typeof(GlobalRes))]
        year,
        [Display(Name = "LastTwoYear", ResourceType = typeof(GlobalRes))]
        twoyear,
        [Display(Name = "LastThreeYear", ResourceType = typeof(GlobalRes))]
        threeyear
    };

    public enum Filter
    {
        [Display(Name = "MostPopul", ResourceType = typeof(GlobalRes))]
        Popularity = 1,
        [Display(Name = "MostComment", ResourceType = typeof(GlobalRes))]
        Comments = 2,
        [Display(Name = "ByPriceAsc", ResourceType = typeof(GlobalRes))]
        PriceAsc = 3,
        [Display(Name = "ByPriceDesc", ResourceType = typeof(GlobalRes))]
        PriceDesc = 4,
        [Display(Name = "ByNew", ResourceType = typeof(GlobalRes))]
        New = 5
    }
}
