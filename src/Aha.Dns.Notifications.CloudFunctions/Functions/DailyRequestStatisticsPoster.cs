using System;
using System.Threading.Tasks;
using Aha.Dns.Notifications.CloudFunctions.ApiClients;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Serilog;

namespace Aha.Dns.Notifications.CloudFunctions.Functions
{
    public class DailyRequestStatisticsPoster
    {
        private const string ServerNameAll = "all";

        private readonly ISummarizedStatisticsApiClient _summarizedStatisticsApiClient;
        private readonly ILogger _logger;

        public DailyRequestStatisticsPoster(
            ISummarizedStatisticsApiClient summarizedStatisticsApiClient)
        {
            _summarizedStatisticsApiClient = summarizedStatisticsApiClient;
            _logger = Log.ForContext("SourceContext", nameof(DailyRequestStatisticsPoster));
        }

        [FunctionName("DailyRequestStatisticsPoster")]
        public async Task RunAsync([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer)
        {
            var summarizedStatistics = await _summarizedStatisticsApiClient.GetSummarizedDnsServerStatistics(ServerNameAll);
            var t = 2;
        }
    }
}
