﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Publisher : IEntityNamed
    {
        public Publisher()
        {
            Games = new List<Game>();
        }

        public Guid Id { get; set; }
        [StringLength(65)]
        [Index("IP_name", 1, IsUnique = true)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public virtual ICollection<Game> Games { get; set; }
       
    }
}
