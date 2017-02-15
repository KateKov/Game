using System.ComponentModel.DataAnnotations;
using GameStore.Web.App_LocalResources;

namespace GameStore.Web.ViewModels
{
    public class CommentViewModel
    {
        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "CommentIdError")]
        public string Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "CommentNameError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessageResourceType = typeof(GlobalRes), ErrorMessageResourceName = "CommentBodyError")]
        [Display(ResourceType = typeof(GlobalRes), Name = "Body")]
        public string Body { get; set; }

        [Display(ResourceType = typeof(GlobalRes), Name = "Game")]
        public string GameId { get; set; }

        public string GameKey { get; set; }

        public string ParentCommentName { get; set; }

        public string ParentCommentId { get; set; }

        public string Quote { get; set; }

        public bool IsDeleted { get; set; }
    }
}
