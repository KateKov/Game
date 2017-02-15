using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameStore.DAL.Entities
{
    public class Ban : EntityBase
    {

        public string Reason { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsCanceled { get; set; }

        public virtual User User { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
    }
}
