using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace RetreatSchedule.Poco.Paystack
{
    public class History
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "time")]
        public int Time { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}
