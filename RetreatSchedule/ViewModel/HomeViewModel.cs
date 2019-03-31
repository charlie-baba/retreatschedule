using System.Collections.Generic;
using RetreatSchedule.Poco;
using RetreatSchedule.Models;

namespace RetreatSchedule.ViewModel
{
    public class HomeViewModel
    {
        public PaginatedList<Activity> Activities { get; set; }

        public List<CountData> Locations { get; set; }

        public List<CountData> ActivityTypes { get; set; }
    }

}
