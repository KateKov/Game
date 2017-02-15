using System;
using System.Collections.Generic;
using GameStore.BLL.DTO.Translation;

namespace GameStore.BLL.Interfaces.Translates
{
    public enum Translates
    {
        PublisherDTO,
        GameDTO,
        GenreDTO,
        PlatformTypeDTO,
        RoleDTO
    };

    public static class Translate<T> where T: class, IDtoBase, new()
    {
        private static readonly Dictionary<Translates, IDtoNamed> _translates;

        static Translate()
        {
            _translates = new Dictionary<Translates, IDtoNamed>()
            {
                {Translates.GameDTO, new GameDTOTranslate()},
                {Translates.GenreDTO, new GenreDTOTranslate()},
                { Translates.PublisherDTO, new PublisherDTOTranslate()},
                {Translates.PlatformTypeDTO, new PlatformTypeDTOTranslate()},
                {Translates.RoleDTO, new RoleDTOTranslate() }
            };
        }
        public static bool IsTranslate()
        {
            var name = typeof(T).Name;
            return Enum.IsDefined(typeof(Translates), name);
        }

        public static IDtoNamed GetTranslateType()
        {
            if (!IsTranslate())
            {
                return null;
            }

            var translate = (Translates) Enum.Parse(typeof(Translates), typeof(T).Name);
            return _translates[translate];         
        }
    }
}
