using System.Collections.Generic;
using System.Linq;
using GameStore.BLL.Interfaces.MailServer;
using GameStore.DAL.Entities;
using GameStore.DAL.Enums;
using GameStore.DAL.Interfaces;

namespace GameStore.BLL.Infrastructure.MailServer
{
    public class MailServer : IObservable
    {
        private readonly List<IObserver> _observers;

        private readonly IUnitOfWork _unitOfWork;

        private MailServer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _observers = new List<IObserver>();
        }

        public void AddObserver(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void NotifyObserver(Order order)
        {
            var users = _unitOfWork.Repository<User>().FindBy(u => u.Roles.Any(r => r.Translates.First(x=>x.Language == Language.en).Name == "Manager")).ToList();
            foreach (var observer in _observers)
            {
                var observer1 = observer;
                observer.Notify(order, users.Where(u => u.ManagerProfile == null ?
                    NotificationMethod.Mail == observer1.Method : u.ManagerProfile.Method == observer1.Method));
            }
        }

        public static MailServer CreateServer(IUnitOfWork unitOfWork)
        {
            var observable = new MailServer(unitOfWork);
            observable.AddObserver(new MailNotification());
            observable.AddObserver(new MobileAppNotification());
            observable.AddObserver(new SmsNotification());
            return observable;
        }
    }
}
