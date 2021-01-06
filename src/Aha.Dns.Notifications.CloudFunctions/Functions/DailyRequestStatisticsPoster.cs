using Aha.Dns.Notifications.CloudFunctions.ApiClients;
using Aha.Dns.Notifications.CloudFunctions.Extensions;
using Aha.Dns.Notifications.CloudFunctions.NotificationClients;
using Microsoft.Azure.WebJobs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.Functions
{
    public class DailyRequestStatisticsPoster
    {
        private const string ServerNameAll = "all";

        private readonly ISummarizedStatisticsApiClient _summarizedStatisticsApiClient;
        private readonly IEnumerable<INotificationClient> _notificationClients;
        private readonly ILogger _logger;

        public DailyRequestStatisticsPoster(
            ISummarizedStatisticsApiClient summarizedStatisticsApiClient,
            IEnumerable<INotificationClient> notificationClients)
        {
            _summarizedStatisticsApiClient = summarizedStatisticsApiClient;
            _notificationClients = notificationClients;
            _logger = Log.ForContext("SourceContext", nameof(DailyRequestStatisticsPoster));
        }

        [FunctionName("DailyRequestStatisticsPoster")]
        public async Task RunAsync([TimerTrigger("0 0 19 * * *")] TimerInfo myTimer)
        {
            try
            {
                var timeSpan = GetStatisticsTimeSpan();
                var printableTimeSpan = timeSpan.GetPrintableTimeSpan();
                var summarizedStatistics = await _summarizedStatisticsApiClient.GetSummarizedDnsServerStatistics(ServerNameAll, timeSpan);

                foreach (var notificationClient in _notificationClients)
                {
                    if (!(await notificationClient.TrySendDnsStatistics(summarizedStatistics, printableTimeSpan)))
                        _logger.Warning("NotificationClient {Client} returned false when sending DNS statistics {@Statistics}", notificationClient.Integration, summarizedStatistics);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Got an exception while running {FunctionName}", nameof(DailyRequestStatisticsPoster));
                throw;
            }
        }

        private TimeSpan GetStatisticsTimeSpan()
        {
            var dateTime = DateTime.UtcNow;

            if (dateTime.Month == 1 && dateTime.Day == 1)
                return TimeSpan.FromDays(365);
            if (dateTime.Day == 1)
                return TimeSpan.FromDays(30);
            if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                return TimeSpan.FromDays(7);
            else
                return TimeSpan.FromDays(1);
        }
    }
}
