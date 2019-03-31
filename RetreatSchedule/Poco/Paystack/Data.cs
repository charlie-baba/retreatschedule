using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class Data
    {
        [DataMember(Name = "id")]
        public long ID { get; set; }

        [DataMember(Name = "amount")]
        public long Amount { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }

        [DataMember(Name = "transaction_date")]
        public string TransactionDate { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }

        [DataMember(Name = "reference")]
        public string Reference { get; set; }

        [DataMember(Name = "domain")]
        public string Domain { get; set; }

        [DataMember(Name = "metadata")]
        public MetaData Metadata { get; set; }

        [DataMember(Name = "gateway_response")]
        public string GatewayResponse { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
        
        [DataMember(Name = "paid_at")]
        public string PaidAt { get; set; }

        [DataMember(Name = "created_at")]
        public string CreatedAt { get; set; }

        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "ip_address")]
        public string IpAddress { get; set; }

        [DataMember(Name = "log")]
        public Log Log { get; set; }

        [DataMember(Name = "fees")]
        private long Fees { get; set; }

        [DataMember(Name = "plan")]
        private string Plan { get; set; }

        [DataMember(Name = "authorization")]
        private Authorization Authorization { get; set; }

        [DataMember(Name = "customer")]
        private Customer Customer { get; set; }
    }
}
