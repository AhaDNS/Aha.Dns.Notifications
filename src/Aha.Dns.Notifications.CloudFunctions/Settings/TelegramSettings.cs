namespace Aha.Dns.Notifications.CloudFunctions.Settings
{
    public class TelegramSettings
    {
        public const string ConfigSectionName = "TelegramSettings";

        public string Token { get; set; }
        public string TelegramUrl { get; set; }
        public string TelegramChannel { get; set; }
    }
}
