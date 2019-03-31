using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class PaystackVerificationResponse
    {
        [DataMember(Name = "status")]
        public bool Status { get; set; }
        
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "data")]
        public Data Data { get; set; }
    }
}
