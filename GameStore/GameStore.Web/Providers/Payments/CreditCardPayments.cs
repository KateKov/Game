using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameStore.DAL.Enums;
using GameStore.Web.PaymentService;
using PaymentStatus = GameStore.Web.PaymentService.PaymentStatus;

namespace GameStore.Web.Providers.Payments
{
    public class CreditCardPayments
    {
        private readonly  IPaymentService _paymentService;
        private readonly Dictionary<CardType, Func<PaymentParams, Task<PaymentStatus>>> _dictionary;

        public CreditCardPayments(IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _dictionary = new Dictionary<CardType, Func<PaymentParams, Task<PaymentStatus>>>
            {
                {CardType.Visa, _paymentService.PayByVisaAsync },
                {CardType.MasterCard, _paymentService.PayByMasterCardAsync }
            };
        }

        public Func<PaymentParams, Task<PaymentStatus>> Pay(CardType cardType)
        {
            return _dictionary[cardType];
        }
    }
}