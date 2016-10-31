using FluentValidation.Attributes;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Entities
{
    //[Validator(typeof(Infrastracture.Validators.GenreValidator))]
    public class Genre: IEntityBase
    {
       
        [Key]
        public int Id { get; set; }
        [DisplayName("Жанр")]
        [Required]
        [StringLength(65)]
        [Index("IX_key", 1, IsUnique = true)]
        public string Name { get; set; }
        [DisplayName("Название жанра-родителя")]
        public int? ParentId { get; set; }


        public virtual ICollection<Game> Games { get; set; }

        //public Genre()
        //{
        //    Games=new List<Game>();
        //}
    }
}
