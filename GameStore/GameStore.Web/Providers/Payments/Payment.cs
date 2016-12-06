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
        private IPaymentStrategy _payments;
        private static readonly Dictionary<PaymentTypes, IPaymentStrategy> _dictionary;

        public enum PaymentTypes{
            Bank,
            IBox, 
            Visa
        }


        static Payment()
        {
            _dictionary = new Dictionary<PaymentTypes, IPaymentStrategy>()
            {
                {PaymentTypes.Bank, new Bank() },
                {PaymentTypes.IBox, new IBox() },
                {PaymentTypes.Visa, new Visa() }
            };
          
        }

        public Payment(PaymentTypes payment)
        {
            if (payment.HasFlag(PaymentTypes.Bank) || payment.HasFlag(PaymentTypes.IBox) || payment.HasFlag(PaymentTypes.Visa))
            {
                _payments = _dictionary[payment];
            }
        }      
      
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return _payments.Pay(order, viewResult);
        }
    }
}