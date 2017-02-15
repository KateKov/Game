using System;
using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Interfaces.Translates;

namespace GameStore.BLL.DTO
{
    public class GameDTO : IDtoBase, ITranslateDTODescriptioned<GameDTOTranslate>, ITranslateDTONamed<GameDTOTranslate>
    {
        public string Id { get; set; }
        public string Key { get; set; }   
        public decimal Price { get; set; }
        public short UnitsInStock { get; set; }
        public bool Discountinues { get; set; }
        public string PublisherId { get; set; }
        public string FilePath { get; set; }
        public int Viewing { get; set; }
        public DateTime DateOfAdding { get; set; }
        public ICollection<string> GenresId { get; set; }
        public ICollection<string> TypesId { get; set; }
        public ICollection<string> Comments { get; set; }
        public ICollection<GameDTOTranslate> Translates { get; set; }
        public bool IsDeleted { get; set; }

    }
}
