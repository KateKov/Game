using System.Runtime.Serialization;

namespace GameStore.WcfService.Models
{
    [DataContract(Name = "PaymentParams")]
    public class Payment
    {
        [DataMember(IsRequired = true)]
        public string FromCardNumber { get; set; }

        [DataMember(IsRequired = true)]
        public string ToCardNumber { get; set; }

        [DataMember(IsRequired = true)]
        public string NameSurname { get; set; }

        [DataMember(IsRequired = true)]
        public int CvvCode { get; set; }

        [DataMember(IsRequired = true)]
        public int ExpirationMonth { get; set; }

        [DataMember(IsRequired = true)]
        public int ExpirationYear { get; set; }

        [DataMember(IsRequired = true)]
        public string Purpose { get; set; }

        [DataMember(IsRequired = true)]
        public decimal Amount { get; set; }

        [DataMember(IsRequired = true)]
        public string Token { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }
    }
}