using System.Collections.Generic;

namespace GameStore.Web.ViewModels
{
    public class PublisherViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public ICollection<GameViewModel> Games { get; set; }
    }
}