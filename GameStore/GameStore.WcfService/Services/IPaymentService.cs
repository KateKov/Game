using System;
using System.Collections.Generic;
using System.ServiceModel;
using GameStore.WcfService.Enums;
using GameStore.WcfService.Models;

namespace GameStore.WcfService.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPaymentService" in both code and config file together.
    [ServiceContract]
    public interface IPaymentService
    {
        [OperationContract]
        PaymentStatus PayByVisa(Payment data);

        [OperationContract]
        PaymentStatus PayByMasterCard(Payment data);

        [OperationContract]
        PaymentStatus Confirm(Guid transferId, string secureCode);

        [OperationContract]
        IEnumerable<Transfer> GetHistoryByCard(string cardNumber);
    }
}
