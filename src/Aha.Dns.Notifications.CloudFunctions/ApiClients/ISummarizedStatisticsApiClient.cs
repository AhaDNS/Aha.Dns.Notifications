using Aha.Dns.Notifications.CloudFunctions.Models;
using System;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.ApiClients
{
    public interface ISummarizedStatisticsApiClient
    {
        public Task<SummarizedDnsServerStatistics> GetSummarizedDnsServerStatistics(string server, TimeSpan timeSpan);
    }
}
