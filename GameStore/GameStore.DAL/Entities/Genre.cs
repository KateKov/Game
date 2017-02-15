using System;
using System.Collections.Generic;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Genre : EntityBase, ITranslateNamed<GenreTranslate>
    {
        public Guid? ParentId { get; set; }
     
        public virtual Genre ParentGenre { get; set; }
      
        public virtual ICollection<Genre> ChildGenres { get; set; }
     
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<GenreTranslate> Translates { get; set; }
    }
}