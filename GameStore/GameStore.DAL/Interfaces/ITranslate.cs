using System;
using GameStore.DAL.Enums;

namespace GameStore.DAL.Interfaces
{
    public interface ITranslate : IEntityNamed
    {
        Language Language { get; set; }
        Guid? BaseEntityId { get; set; }
    }
}
