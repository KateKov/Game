using System.Collections.Generic;
using GameStore.BLL.Interfaces.MailServer;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;

namespace GameStore.BLL.Infrastructure.MailServer
{
    public class SmsNotification : IObserver
    {
        public void Notify(Order order, IEnumerable<User> users)
        {
            return;
        }

        public NotificationMethod Method
        {
            get { return NotificationMethod.Sms; }
        }
    }
}
