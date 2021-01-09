using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ForensicsObjects
{
    public class ForensicsTimestampParser
    {
        public static long Parse(string timestampStr)
        {
            return TimeSpan.Parse(timestampStr, CultureInfo.InvariantCulture).Ticks;
        }
    }
}
