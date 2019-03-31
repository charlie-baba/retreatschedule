using System.Collections.Generic;
using System.Diagnostics;

namespace RetreatSchedule.Models
{
    public class ActivityType : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Activity> Activities { get; set; }
    }
}
