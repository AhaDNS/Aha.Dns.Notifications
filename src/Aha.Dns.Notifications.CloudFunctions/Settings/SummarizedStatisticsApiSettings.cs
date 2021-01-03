using System;
using System.Collections.Generic;
using System.Text;

namespace Aha.Dns.Notifications.CloudFunctions.Settings
{
    public class SummarizedStatisticsApiSettings
    {
        public const string ConfigSectionName = "SummarizedStatisticsApiSettings";

        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
