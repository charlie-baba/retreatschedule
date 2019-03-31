using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Poco
{
    public class ChartsData
    {
        public List<DonutChartData> DonutChartDatas { get; set; }

        public List<AreaChartData> AreaChartDatas { get; set; }
        
        public List<AreaChartData> VisitorChartDatas { get; set; }

        public string[][] CalendarChartDatas { get; set; }
    }
}
