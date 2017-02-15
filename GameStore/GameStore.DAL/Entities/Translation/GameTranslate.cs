using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class GameTranslate : EntityTranslate, ITranslateWithDescription
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public virtual Game Game { get; set; }
        [ForeignKey("Game")]
        public Guid? BaseEntityId { get; set; }
    }
}
