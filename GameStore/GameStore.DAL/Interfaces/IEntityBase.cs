using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.DAL.Interfaces
{
    public interface IEntityBase
    {
       Guid EntityId { get; set; }
        string Id { get; set; }
    }
}
