using GameStore.DAL.Entities;

namespace GameStore.BLL.Interfaces.MailServer
{
    public interface IObservable
    {
        void AddObserver(IObserver observer);

        void RemoveObserver(IObserver observer);

        void NotifyObserver(Order order);
    }
}
