using System.ComponentModel;

namespace RetreatSchedule.Models
{
    public class Setting: BaseEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
