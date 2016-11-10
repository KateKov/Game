using System.Collections.Generic;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Publisher : IEntityBase
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}
