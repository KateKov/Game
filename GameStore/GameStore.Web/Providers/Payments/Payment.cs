using System;
using System.Collections.Generic;
using System.Web.Mvc;
using GameStore.Web.ViewModels.Orders;
using GameStore.DAL.Enums;

namespace GameStore.Web.Providers.Payments
{
    public class Payment
    {
        private readonly IPaymentStrategy _payments;
        private static readonly Dictionary<PaymentTypes, IPaymentStrategy> Dictionary;

        static Payment()
        {
            Dictionary = new Dictionary<PaymentTypes, IPaymentStrategy>()
            {
                {PaymentTypes.Bank, new Bank() },
                {PaymentTypes.Box, new Box() },
                {PaymentTypes.CardPay, new CardPay() }
            };

        }

        public Payment(PaymentTypes payment)
        {
           _payments = Dictionary[payment];       
        }      
      
        public ActionResult Pay(OrderViewModel order, Func<string, object, ViewResult> viewResult)
        {
            return _payments.Pay(order, viewResult);
        }
    }
}