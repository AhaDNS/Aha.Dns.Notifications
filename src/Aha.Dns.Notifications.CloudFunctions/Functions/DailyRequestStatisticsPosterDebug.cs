#if DEBUG
using Aha.Dns.Notifications.CloudFunctions.ApiClients;
using Aha.Dns.Notifications.CloudFunctions.NotificationClients;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.Functions
{
    public class DailyRequestStatisticsPosterDebug
    {
        private readonly ISummarizedStatisticsApiClient _summarizedStatisticsApiClient;
        private readonly IEnumerable<INotificationClient> _notificationClients;

        public DailyRequestStatisticsPosterDebug(
            ISummarizedStatisticsApiClient summarizedStatisticsApiClient,
            IEnumerable<INotificationClient> notificationClients)
        {
            _summarizedStatisticsApiClient = summarizedStatisticsApiClient;
            _notificationClients = notificationClients;
        }

        [FunctionName("DailyRequestStatisticsPosterDebug")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var dailyRequestStatisticsPoster = new DailyRequestStatisticsPoster(_summarizedStatisticsApiClient, _notificationClients);
            await dailyRequestStatisticsPoster.RunAsync(default);

            return new OkObjectResult("Done");
        }
    }
}
#endif
