using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanword.Business.Builders.RssFeeds {
    public class RssFeedDateTimeParser {
        /// <summary>
        /// Below definitions are accurate regardless of Daylight Savings Time - The 'DT' in 'PDT' means 'Daylight Time' and accounts for DST conversions
        /// </summary>
        private Dictionary<string, int> USATimezones = new Dictionary<string, int>() {

            {"PDT", -7},
            {"PST", -8},
            {"MDT", -6},
            {"MST", -7},
            {"CDT", -5},
            {"CST", -6},
            {"EDT", -4},
            {"EST", -5},
            {"AKDT", -8},
            {"AKST", -9},
            {"HADT", -9},
            {"HDT", -9 },
            {"HAST", -10},
            {"HST", -10},
			{"GMT", 0 },

            // Best guess for timezone
            { "PT", -7},
            { "MT", -6 },
            { "CT", -5 },
            { "ET", -4 }
        };

        /// <summary>
        /// Convert string to DateTime
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public DateTime Parse(string datetime) {

            if (string.IsNullOrEmpty(datetime))
                return DateTime.UtcNow;

            // See if string contains a TimeZone abbreviation
            var timezone = USATimezones.Select(i => i.Key).FirstOrDefault(i => datetime.Contains(i));

            // Replace TimeZone abbreviation with UTC offset
            if (!string.IsNullOrEmpty(timezone)) {

                datetime = datetime.Replace(timezone, USATimezones[timezone]==0 ? "-0": USATimezones[timezone].ToString());
            }
			try
			{
				return DateTime.Parse(datetime).ToUniversalTime();
			}
			catch { return DateTime.UtcNow; }
        }
    }
}
