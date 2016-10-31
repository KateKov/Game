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
    //[Validator(typeof(Infrastracture.Validators.TypeValidator))]
    public class PlatformType: IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Тип")]
        [StringLength(65)]
        [Index("IY_key", 1, IsUnique = true)]
        public string Type { get; set; }
        public virtual ICollection<Game> Games { get; set; }

        //public PlatformType()
        //{
        //    //Games=new List<Game>();
        //}
    }
}
