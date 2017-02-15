namespace GameStore.WcfService.Models
{
    public class Card
    {
        public string CardNumber { get; set; }

        public int CvvCode { get; set; }

        public int ExpirationMonth { get; set; }

        public int ExpirationYear { get; set; }


        public decimal AmountOfMoney { get; set; }

        public User User { get; set; }
    }
}