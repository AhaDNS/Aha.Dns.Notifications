using System;

namespace Aha.Dns.Notifications.CloudFunctions.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string GetPrintableTimeSpan(this TimeSpan timeSpan)
        {
            string result;

            if (timeSpan.Days == 365)
                result = "year";

            else if (timeSpan.Days == 30)
                result = "month";

            else if (timeSpan.Days == 7)
                result = "week";

            else if (timeSpan.Days == 1)
                result = "24h";

            else if (timeSpan.Days > 1)
                result = $"{timeSpan.Days} days";

            else
                result = $"{timeSpan.Hours}h";

            return result;
        }
    }
}
