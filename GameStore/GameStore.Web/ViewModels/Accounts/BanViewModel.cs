using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Enums;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels.Accounts
{
    public class BanViewModel
    {
        public string Name { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "BanOn")]
        public Duration Duration { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Reason")]
        public string Reason { get; set; }
    }
}