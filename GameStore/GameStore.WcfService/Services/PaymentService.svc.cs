using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameStore.WcfService.Enums;
using GameStore.WcfService.Interfaces;
using GameStore.WcfService.Models;
using GameStore.WcfService.Repositories;
using GameStore.WcfService.Services.Senders;
using Newtonsoft.Json;

namespace GameStore.WcfService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PaymentService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PaymentService.svc or PaymentService.svc.cs at the Solution Explorer and start debugging.
    public class PaymentService : IPaymentService
    {
        private const string SecureCode = "48975";

        static PaymentService()
        {
            PaymentInitializer.Seed();
        }

        public PaymentStatus PayByVisa(Payment model)
        {
            return Pay(model);
        }

        public PaymentStatus PayByMasterCard(Payment model)
        {
            return Pay(model);
        }

        public PaymentStatus Confirm(Guid transferId, string secureCode)
        {
            Transfer transfer = PaymentContext.Transfers.First(x => x.TransferId == transferId);

            if (transfer.SecureCode == SecureCode)
            {
                ExecuteTransfer(transfer);
            }

            return transfer.Status;
        }

        public IEnumerable<Transfer> GetHistoryByCard(string cardNumber)
        {
            IEnumerable<Transfer> transfers =
                PaymentContext.Transfers.Where(x => x.FromCardNumber == cardNumber || x.ToCardNumber == cardNumber)
                    .ToList();

            return transfers;
        }


        private PaymentStatus Validate(Payment model)
        {
            if (model == null || (string.IsNullOrEmpty(model.FromCardNumber) && string.IsNullOrEmpty(model.ToCardNumber) &&
               PaymentContext.Cards.Any(x => x.CardNumber == model.ToCardNumber)))
            {
                return PaymentStatus.CardDoesnotExist;
            }

            if (PaymentContext.Cards.First(x => x.CardNumber == model.FromCardNumber).AmountOfMoney < model.Amount)
            {
                return PaymentStatus.NotEnoughMoney;
            }

            if (model.ExpirationMonth < DateTime.UtcNow.Month && model.ExpirationYear <= DateTime.UtcNow.Year)
            {
                return PaymentStatus.Failed;
            }

            return PaymentStatus.Succesful;
        }

        private PaymentStatus Pay(Payment model)
        {
            var status = Validate(model);
            var code = Guid.NewGuid().ToString();
            var transfer = new Transfer
            {
                TransferId = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                Status = status,
                AmountOfTransfer = model.Amount,
                FromCardNumber = model.FromCardNumber,
                Purpose = model.Purpose,
                SecureCode = code,
                ToCardNumber = model.ToCardNumber
            };
         
            if (status != PaymentStatus.Succesful)
            {
                SaveInfo(transfer);
                return status;
            }

            PaymentContext.Transfers.Add(transfer);

            if (!string.IsNullOrEmpty(model.PhoneNumber))
            {
                SendConfirmCode(model.PhoneNumber);
                Confirm(transfer.TransferId, code);
            }

            ExecuteTransfer(transfer);

            SendEmailReport(transfer, model.Email);

            SaveInfo(transfer);
            return transfer.Status;
        }

        private void SendConfirmCode(string phoneNumber)
        {
            string secureCode = SecureCode;

            ISender phoneSender = new PhoneSender(phoneNumber, secureCode);
            phoneSender.Send();
        }

        private void SendEmailReport(Transfer transfer, string email)
        {
            ISender emailSender = new EmailSender(email, transfer);
            emailSender.Send();
            if (string.IsNullOrEmpty(email)) return;
            emailSender = new EmailSender(email, transfer);
            emailSender.Send();
        }

        private void ExecuteTransfer(Transfer transfer)
        {
            Card from =  PaymentContext.Cards.First(x => x.CardNumber == transfer.FromCardNumber);
            Card to = PaymentContext.Cards.First(x => x.CardNumber == transfer.ToCardNumber);

            if (from.AmountOfMoney < transfer.AmountOfTransfer)
            {
                transfer.Status = PaymentStatus.NotEnoughMoney;
                return;
            }

            from.AmountOfMoney -= transfer.AmountOfTransfer;
            to.AmountOfMoney += transfer.AmountOfTransfer;

            transfer.Status = PaymentStatus.Succesful;
        }

        private void SaveInfo(Transfer transfer)
        {
            string serialized = JsonConvert.SerializeObject(transfer, Formatting.Indented);
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var fileName = "Story.json";
            using (var fileStream = new FileStream($"{path}/{fileName}", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(serialized);
                fileStream.Seek(0, SeekOrigin.End);
                fileStream.Write(array, 0, array.Length);
            }
        }
    }
}