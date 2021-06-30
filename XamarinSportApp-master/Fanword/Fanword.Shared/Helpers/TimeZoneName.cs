using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Shared.Helpers
{
    public class TimeZoneName
    {
        public static string GetLocalTimezoneName()
        {
            if (TimeZoneInfo.Local.SupportsDaylightSavingTime)
            {
                if (TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now))
                {
                    return TimeZoneInfo.Local.DaylightName;
                }
                else
                {
                    return TimeZoneInfo.Local.StandardName;
                }
            }
            else
            {
                return TimeZoneInfo.Local.StandardName;
            }
        }
    }
}
