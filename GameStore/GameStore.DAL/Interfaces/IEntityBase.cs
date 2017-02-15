using System;

namespace GameStore.DAL.Interfaces
{
    public interface IEntityBase
    {
       Guid Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
