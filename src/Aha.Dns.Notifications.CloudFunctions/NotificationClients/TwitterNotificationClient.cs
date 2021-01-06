using Aha.Dns.Notifications.CloudFunctions.Models;
using Aha.Dns.Notifications.CloudFunctions.Settings;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Tweetinvi;

namespace Aha.Dns.Notifications.CloudFunctions.NotificationClients
{
    public class TwitterNotificationClient : INotificationClient
    {
        public const string IntegrationName = "Twitter";
        public string Integration => IntegrationName;

        private readonly TwitterClient _twitterClient;
        private readonly ILogger _logger;

        public TwitterNotificationClient(IOptions<TwitterSettings> twitterSettings)
        {
            _twitterClient = new TwitterClient(twitterSettings.Value.ConsumerKey, twitterSettings.Value.ConsumerSecret, twitterSettings.Value.AccessToken, twitterSettings.Value.AccessTokenSecret);
            _logger = Log.ForContext("SourceContext", nameof(TwitterNotificationClient));
        }

        public async Task<bool> TrySendDnsStatistics(SummarizedDnsServerStatistics summarizedDnsServerStatistics, string printableTimeSpan)
        {
            try
            {
                var queriesRequested = summarizedDnsServerStatistics.QueriesRequested.ToString("n0", new CultureInfo("en-US"));
                var queriesBlocked = summarizedDnsServerStatistics.QueriesBlocked.ToString("n0", new CultureInfo("en-US"));
                var message = $"During the last {printableTimeSpan}, AhaDNS.com have served {queriesRequested} DNS requests and protected our users from {queriesBlocked} malicious requests!";

                // For now, just hard-code a few hashtags. Make these configurable later
                message += "\n\n#AhaDNS #EncryptedDNS #AdBlockDNS #DNS #DNSoverHTTPS #DNSoverTLS #DoH #DoT #adblock #Privacy #Ads #FOSS";

                _ = await _twitterClient.Tweets.PublishTweetAsync(message);
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Got an exception while sending DNS statistics alert to Twitter");
                return false;
            }
        }
    }
}
