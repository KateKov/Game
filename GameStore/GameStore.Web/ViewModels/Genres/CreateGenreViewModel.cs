using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;
using GameStore.Web.ViewModels.Translates;

namespace GameStore.Web.ViewModels.Genres
{
    public class CreateGenreViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "ErrorName")]
        [MaxLength(50, ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "NameRangeError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        public IList<TranslateViewModel> Translates { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "ParentGenre")]
        public string ParentName { get; set; }
        public IEnumerable<GenreViewModel> AllGenres { get; set; }
    }
}