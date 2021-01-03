using Aha.Dns.Notifications.CloudFunctions.Models;
using Aha.Dns.Notifications.CloudFunctions.Settings;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.ApiClients
{
    public class SummarizedStatisticsApiClient : ISummarizedStatisticsApiClient
    {
        private readonly SummarizedStatisticsApiSettings _summarizedStatisticsApiSettings;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;

        public SummarizedStatisticsApiClient(
            IOptions<SummarizedStatisticsApiSettings> summarizedStatisticsApiSettings,
            HttpClient httpClient)
        {
            _summarizedStatisticsApiSettings = summarizedStatisticsApiSettings.Value;
            _httpClient = httpClient;
            _logger = Log.ForContext("SourceContext", nameof(SummarizedStatisticsApiClient));
        }

        public async Task<SummarizedDnsServerStatistics> GetSummarizedDnsServerStatistics(string server)
        {
            _logger.Debug("Getting summarized statistics for server {Server}", server);

            try
            {
                var queryParameters = new Dictionary<string, string>
                {
                    { "code", _summarizedStatisticsApiSettings.ApiKey },
                    { "server", server },
                    { "timespan", TimeSpan.FromDays(2).ToString() }
                };

                var apiUrl = $"{_summarizedStatisticsApiSettings.BaseUrl}/api/SummarizedStatisticsApi";
                var requestUri = QueryHelpers.AddQueryString(apiUrl, queryParameters);
                _logger.Debug("Sending GET -> {Url}", apiUrl);

                var httpResponse = await _httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();

                return JsonConvert.DeserializeObject<SummarizedDnsServerStatistics>(await httpResponse.Content.ReadAsStringAsync());
            }
            catch (HttpRequestException hre)
            {
                _logger.Error(hre, "Unsuccessful response when retrieving summarized statistics for server {server}", server);
                throw;
            }
            catch (Exception e)
            {
                _logger.Error(e, "Got an unhandled exception while fetching summarized statistics for server {server}", server);
                throw;
            }
        }
    }
}
