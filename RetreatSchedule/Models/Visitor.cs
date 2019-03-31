using System;
using RetreatSchedule.Models;
using RetreatSchedule.Models.Enum;

namespace RetreatSchedule.Models
{
    public class Visitor : BaseEntity
    {
        public string IpAddress { get; set; }

        public DateTime Date { get; set; }

        public int? UserId { get; set; }

        public Page Page { get; set; }
    }
}
