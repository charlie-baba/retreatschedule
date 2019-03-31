using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Poco
{
    public class AreaChartData
    {
        public string Key { get; set; }

        public string Color { get; set; }

        public List<double[]> Values { get; set; }
    }
}
