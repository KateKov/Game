using System;
using GameStore.DAL.Entities.Translation;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Interfaces
{
    public interface ITranslate
    {
        Language Language { get; set; }
    }
}
