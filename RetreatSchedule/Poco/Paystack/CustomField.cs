using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class CustomField
    {
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }

        [DataMember(Name = "variable_name")]
        public string VariableName { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
