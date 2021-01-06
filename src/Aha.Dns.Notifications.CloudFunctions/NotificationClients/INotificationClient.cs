using Aha.Dns.Notifications.CloudFunctions.Models;
using System.Threading.Tasks;

namespace Aha.Dns.Notifications.CloudFunctions.NotificationClients
{
    public interface INotificationClient
    {
        /// <summary>
        /// Integration that the client integrates with, i.e. Telegram, Twitter etc.
        /// </summary>
        string Integration { get; }

        /// <summary>
        /// Send DNS query statistics
        /// </summary>
        /// <param name="summarizedDnsServerStatistics"></param>
        /// <param name="printableTimeSpan"></param>
        /// <returns></returns>
        Task<bool> TrySendDnsStatistics(SummarizedDnsServerStatistics summarizedDnsServerStatistics, string printableTimeSpan);
    }
}
