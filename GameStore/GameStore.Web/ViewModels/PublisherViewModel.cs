using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GameStore.Web.ViewModels
{
    public class PublisherViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "The name can't be longer than 20 characters")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [MaxLength(1000, ErrorMessage = "The name can't be longer than 1000 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Display(Name = "HomePage")]
        [DataType(DataType.Url, ErrorMessage = "The field isn't a url")]
        [Required]
        public string HomePage { get; set; }
    }
}