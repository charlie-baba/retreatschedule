using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetreatSchedule.Util
{
    public static class StringExt
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            return value.Length <= maxLength ? value : $"{value.Substring(0, maxLength)}...";
        }
    }
}
