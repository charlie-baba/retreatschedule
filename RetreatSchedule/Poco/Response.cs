using RetreatSchedule.Models.Enum;

namespace RetreatSchedule.Poco
{
    public class Response
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public string Ref { get; set; }

        public string Email { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
