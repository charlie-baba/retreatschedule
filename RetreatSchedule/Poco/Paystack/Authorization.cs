using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class Authorization
    {
        [DataMember(Name = "authorization_code")]
        public string AuthorizationCode { get; set; }

        [DataMember(Name = "country_code")]
        public string CountryCode { get; set; }

        [DataMember(Name = "card_type")]
        public string CardType { get; set; }

        [DataMember(Name = "last4")]
        public string Last4 { get; set; }

        [DataMember(Name = "exp_month")]
        public string ExpMonth { get; set; }

        [DataMember(Name = "exp_year")]
        public string ExpYear { get; set; }

        [DataMember(Name = "bin")]
        public string Bin { get; set; }

        [DataMember(Name = "bank")]
        public string Bank { get; set; }

        [DataMember(Name = "brand")]
        public string Brand { get; set; }

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "reusable")]
        public bool Reusable { get; set; }

        [DataMember(Name = "signature")]
        public string Signature { get; set; }
    }
}
