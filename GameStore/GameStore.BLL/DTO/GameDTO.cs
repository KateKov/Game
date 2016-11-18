using System.Collections.Generic;
using System.Security.Policy;
using AutoMapper.Configuration.Conventions;
using GameStore.BLL.Interfaces;
using System;

namespace GameStore.BLL.DTO
{
    public class GameDTO : IDtoWithKey, IDtoNamed
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public string PublisherName { get; set; }
        public string PublisherId { get; set; }
        public int Viewing { get; set; }
        public DateTime DateOfAdding { get; set; }
        public ICollection<string> GenresId { get; set; }
        public ICollection<string> GenresName { get; set; }
        public ICollection<string> TypesId { get; set; }
        public ICollection<string> PlatformTypesName { get; set; }
        public ICollection<string> Comments { get; set; }
    }
}
