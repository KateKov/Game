using FluentValidation.Attributes;
using GameStore.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Entities
{
    [Validator(typeof(Infrastracture.Validators.GenreValidator))]
    public class Genre: IEntityBase
    {
       
        [Required]
        public int Id { get; set; }
        [DisplayName("Жанр")]
        public string Name { get; set; }
        [DisplayName("Название жанра-родителя")]
        public int ParentId { get; set; }


        public virtual List<Game> Games { get; set; }

        public Genre()
        {
            Games=new List<Game>();
        }
    }
}
