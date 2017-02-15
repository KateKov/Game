using System;
using GameStore.WcfService.Enums;

namespace GameStore.WcfService.Models
{
    public class Transfer
    {
        public Guid TransferId { get; set; }

        public string FromCardNumber { get; set; }

        public string ToCardNumber { get; set; }

        public string Purpose { get; set; }

        public decimal AmountOfTransfer { get; set; }

        public DateTime Date { get; set; }

        public PaymentStatus Status { get; set; }

        public string SecureCode { get; set; }
    }
}