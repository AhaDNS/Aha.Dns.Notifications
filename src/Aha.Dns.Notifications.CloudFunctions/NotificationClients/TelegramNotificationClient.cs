using Aha.Dns.Notifications.CloudFunctions.Models;
using Aha.Dns.Notifications.CloudFunctions.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.NotificationClients
{
    public class TelegramNotificationClient : INotificationClient
    {
        public const string IntegrationName = "Telegram";
        public string Integration => IntegrationName;

        private readonly HttpClient _httpClient;
        private readonly TelegramSettings _telegramSettings;
        private readonly ILogger _logger;

        public TelegramNotificationClient(HttpClient httpClient, IOptions<TelegramSettings> telegramSettings)
        {
            _httpClient = httpClient;
            _telegramSettings = telegramSettings.Value;
            _logger = Log.ForContext("SourceContext", nameof(TelegramNotificationClient));
        }

        public async Task<bool> TrySendDnsStatistics(SummarizedDnsServerStatistics summarizedDnsServerStatistics, string printableTimeSpan)
        {
            try
            {
                var requestUrl = $"{_telegramSettings.TelegramUrl}/bot{_telegramSettings.Token}/sendMessage";

                var queriesRequested = summarizedDnsServerStatistics.QueriesRequested.ToString("n0", new CultureInfo("en-US"));
                var queriesBlocked = summarizedDnsServerStatistics.QueriesBlocked.ToString("n0", new CultureInfo("en-US"));
                var message = $"During the last {printableTimeSpan}, <a href=\"https://ahadns.com\">AhaDNS.com</a> have served <b>{queriesRequested}</b> DNS requests and protected our users from <b>{queriesBlocked}</b> malicious requests!";

                var requestBody = new StringContent(JsonConvert.SerializeObject(new TelegramRequest(_telegramSettings.TelegramChannel, "HTML", message)), Encoding.UTF8, "application/json");
                var telegramResponse = await _httpClient.PostAsync(requestUrl, requestBody);
                return telegramResponse.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Got an exception while sending DNS statistics alert to Telegram");
                return false;
            }
        }
    }
}
