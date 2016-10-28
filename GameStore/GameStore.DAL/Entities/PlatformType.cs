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
    [Validator(typeof(Infrastracture.Validators.TypeValidator))]
    public class PlatformType: IEntityBase
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName("Тип")]
        public string Type { get; set; }
        public virtual List<Game> Games { get; set; }

        public PlatformType()
        {
            Games=new List<Game>();
        }
    }
}
