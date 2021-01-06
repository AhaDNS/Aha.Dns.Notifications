﻿namespace Aha.Dns.Notifications.CloudFunctions.Settings
{
    public class TwitterSettings
    {
        public const string ConfigSectionName = "TwitterSettings";

        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
        public string AccessToken { get; set; }
        public string AccessTokenSecret { get; set; }
    }
}
