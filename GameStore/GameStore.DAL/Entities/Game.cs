using FluentValidation.Attributes;
using GameStore.DAL.Interfaces;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameStore.DAL.Infrastracture.Validators;

namespace GameStore.DAL.Entities
{
    [Validator(typeof(Infrastracture.Validators.GameValidator))]
    public class Game: IEntityBase
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Key { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Genre> Genres { get; set; }
 
        public virtual List<PlatformType> PlatformTypes { get; set; }

        public Game()
        {
            Comments = new List<Comment>();
            Genres = new List<Genre>();
            PlatformTypes=new List<PlatformType>();
        }
    }
}
