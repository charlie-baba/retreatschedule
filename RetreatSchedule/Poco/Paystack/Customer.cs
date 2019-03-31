using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class Customer
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }

        [DataMember(Name = "customer_code")]
        public string CustomerCode { get; set; }

        [DataMember(Name = "first_name")]
        public string FirstName { get; set; }

        [DataMember(Name = "last_name")]
        public string LastName { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "phone")]
        public string Phone { get; set; }

        [DataMember(Name = "risk_action")]
        public string RiskAction { get; set; }
    }
}
