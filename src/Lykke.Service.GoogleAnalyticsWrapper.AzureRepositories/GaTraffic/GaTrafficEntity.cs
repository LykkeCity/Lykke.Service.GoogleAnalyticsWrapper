using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.GoogleAnalyticsWrapper.AzureRepositories.GaTraffic
{
    public class GaTrafficEntity : TableEntity, IGaTraffic
    {
        public string ClientId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string Keyword { get; set; }
        public string Content { get; set; }

        internal static string GeneratePartitionKey() => "GaTraffic";
        internal static string GenerateRowKey(string clientId) => clientId;

        public static GaTrafficEntity Create(IGaTraffic src)
        {
            return new GaTrafficEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.ClientId),
                ClientId = src.ClientId,
                Source = src.Source,
                Medium = src.Medium,
                Campaign = src.Campaign,
                Keyword = src.Keyword,
                Content = src.Content
            };
        }
    }
}
