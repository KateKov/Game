using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class PublisherTranslate : EntityTranslate, IEntityNamed
    {
        public string Name { get; set; }
        [StringLength(65)]
        [Index("IP_name", 1, IsUnique = true)]
        public string Description { get; set; }
        public virtual Publisher Publisher { get; set; }
    }
}
