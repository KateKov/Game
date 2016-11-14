﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Entities
{
    public class Game : IEntityBase, IEntityNamed, IEntityWithKey
    {
        public Game()
        {
            OrderDetails=new List<OrderDetail>();
            Comments=new List<Comment>();
            Genres=new List<Genre>();
            PlatformTypes=new List<PlatformType>();
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(65)]
        [Index("IX_key", 1, IsUnique = true)]
        public string Key { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public Guid? PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<PlatformType> PlatformTypes { get; set; }
    }
}
