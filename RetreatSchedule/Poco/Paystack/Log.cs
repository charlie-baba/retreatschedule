using System.Collections.Generic;
using System.Runtime.Serialization;

namespace RetreatSchedule.Poco.Paystack
{
    public class Log
    {
        [DataMember(Name = "start_time")]
        public long StartTime { get; set; }

        [DataMember(Name = "time_spent")]
        public int TimeSpent { get; set; }

        [DataMember(Name = "attempts")]
        public int Attempts { get; set; }

        [DataMember(Name = "authentication")]
        public string Authentication { get; set; }

        [DataMember(Name = "errors")]
        public int Errors { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "mobile")]
        public bool Mobile { get; set; }

        [DataMember(Name = "input")]
        public List<string> Input { get; set; }

        //[DataMember(Name = "channel")]
        //public int Channel { get; set; }

        [DataMember(Name = "history")]
        public List<History> Histories { get; set; }
    }
}
