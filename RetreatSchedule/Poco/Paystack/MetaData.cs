using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class MetaData
    {
        [DataMember(Name = "referrer")]
        public string Referrer { get; set; }

        [DataMember(Name = "custom_fields")]
        public List<CustomField> CustomFields { get; set; }

    }
}
