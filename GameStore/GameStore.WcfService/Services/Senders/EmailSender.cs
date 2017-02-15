using GameStore.WcfService.Interfaces;
using GameStore.WcfService.Models;

namespace GameStore.WcfService.Services.Senders
{
    public class EmailSender : ISender
    {
        private readonly string _recieverEmail;
        private readonly Transfer _transfer;

        public EmailSender(string recieverEmail, Transfer transfer)
        {
            _recieverEmail = recieverEmail;
            _transfer = transfer;
        }

        public void Send()
        {
        }
    }
}