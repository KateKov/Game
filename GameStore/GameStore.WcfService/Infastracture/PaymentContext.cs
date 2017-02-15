using System.Collections.Generic;
using GameStore.WcfService.Models;

namespace GameStore.WcfService.Repositories
{
    public class PaymentContext
    {
        static PaymentContext()
        {
            Cards = new List<Card>();
            Transfers = new List<Transfer>();
            Users = new List<User>();
        }

        public static List<User> Users { get; private set; }

        public static List<Card> Cards { get; private set; }

        public static List<Transfer> Transfers { get; private set; }
    }
}