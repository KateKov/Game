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
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DAL.Entities
{
  //  [Validator(typeof(Infrastracture.Validators.GameValidator))]
    public class Game: IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(65)]
        [Index("IX_key", 1, IsUnique = true)]
        public string Key { get; set; }
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
        [DisplayName("Описание")]
        public string Description { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
 
        public virtual ICollection<PlatformType> PlatformTypes { get; set; }

        public Game()
        {
            Comments = new List<Comment>();
            Genres = new List<Genre>();
            PlatformTypes=new List<PlatformType>();
        }
    }
}
