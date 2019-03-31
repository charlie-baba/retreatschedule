using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Util
{
    public static class DateTimeExt
    {
        public static double ToJsTime(this DateTime value)
        {
            if (value == null)
                return 0d;

            return value.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
