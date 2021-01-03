#if DEBUG
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Aha.Dns.Notifications.CloudFunctions.ApiClients;

namespace Aha.Dns.Notifications.CloudFunctions.Functions
{
    public class DailyRequestStatisticsPosterDebug
    {
        private readonly ISummarizedStatisticsApiClient _summarizedStatisticsApiClient;

        public DailyRequestStatisticsPosterDebug(
            ISummarizedStatisticsApiClient summarizedStatisticsApiClient)
        {
            _summarizedStatisticsApiClient = summarizedStatisticsApiClient;
        }

        [FunctionName("DailyRequestStatisticsPosterDebug")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            var dailyRequestStatisticsPoster = new DailyRequestStatisticsPoster(_summarizedStatisticsApiClient);
            await dailyRequestStatisticsPoster.RunAsync(default);

            return new OkObjectResult("Done");
        }
    }
}
#endif