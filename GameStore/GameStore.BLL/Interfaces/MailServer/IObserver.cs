using System.Collections.Generic;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Interfaces.MailServer
{
    public interface IObserver
    {
        void Notify(Order order, IEnumerable<User> users);

        NotificationMethod Method { get; }
    }
}
