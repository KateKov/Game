using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities.Translation
{
    public class GameTranslate : EntityTranslate, IEntityNamed
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public virtual Game Game { get; set; }
    }
}
