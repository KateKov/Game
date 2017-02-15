using GameStore.WcfService.Interfaces;

namespace GameStore.WcfService.Services.Senders
{
    public class PhoneSender : ISender
    {
        private readonly string _phone;
        private readonly string _secureCode;

        public PhoneSender(string phone, string secureCode)
        {
            _phone = phone;
            _secureCode = secureCode;
        }

        public void Send()
        { 
        }
    }
}