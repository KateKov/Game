﻿using System.Collections.Generic;
using GameStore.BLL.Interfaces;

namespace GameStore.BLL.DTO
{
    public class PublisherDTO : IDtoBase
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string Description { get; set; }
        public string HomePage { get; set; }
        public virtual ICollection<GameDTO> Games { get; set; }
    }
}