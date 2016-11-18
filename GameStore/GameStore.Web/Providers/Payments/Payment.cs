using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GameStore.BLL.Infrastructure;
using GameStore.Web.ViewModels;

namespace GameStore.Web.Providers.Payments
{
    public class Payment
    {
        private readonly IPaymentStrategy _payments;
        private static readonly Dictionary<string, IPaymentStrategy> _dictionary;

        static Payment()
        {
            _dictionary = new Dictionary<string, IPaymentStrategy>()
            {
                {"Bank", new Bank() },
                {"IBox", new IBox() },
                {"Visa", new Visa() }
            };
          
        }

        public Payment(string paymentName)
        {
            _payments = _dictionary[paymentName];
        }      
      
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return _payments.Pay(order, viewResult);
        }
    }
}