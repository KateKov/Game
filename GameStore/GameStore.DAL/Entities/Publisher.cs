using System.Collections.Generic;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Interfaces;
using GameStore.DAL.MongoEntities;


namespace GameStore.DAL.Entities
{
    public class Publisher : EntityBase, ITranslateNamed<PublisherTranslate>
    {
        public Publisher()
        {
            Games = new List<Game>();
        }

        public string HomePage { get; set; }
        public virtual ICollection<PublisherTranslate> Translates { get; set; }

        public virtual ICollection<Game> Games { get; set; }
       
    }
}
