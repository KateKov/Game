using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Providers.Payments
{
    public class Payment
    {
        public IPaymentStrategy Payments { private get; set; }

        public Payment()
        {
            _dictionary = new Dictionary<string, IPaymentStrategy>();
            _dictionary.Add("Bank", new Bank());
            _dictionary.Add("IBox", new IBox());
            _dictionary.Add("Visa", new Visa());
        }

        public Payment(string paymentName)
        {
            Payments = _dictionary[paymentName];
        }

        private readonly Dictionary<string, IPaymentStrategy> _dictionary;
      
        public ActionResult Pay(OrderViewModel order)
        {
            return Payments.Pay(order);
        }
    }
}