using System.Collections.Generic;
using GameStore.BLL.Interfaces;
using System;
using GameStore.BLL.DTO.Translation;

namespace GameStore.BLL.DTO
{
    public class GameDTO : IDtoWithKey, ITranslateDTONamed<GameDTOTranslate>
    {
        public string Id { get; set; }
        public string Key { get; set; }
     
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public string PublisherId { get; set; }
        public int Viewing { get; set; }
        public DateTime DateOfAdding { get; set; }
        public ICollection<string> GenresId { get; set; }
        public ICollection<string> TypesId { get; set; }
        public ICollection<string> Comments { get; set; }
        public ICollection<GameDTOTranslate> Translates { get; set; }
    }
}
