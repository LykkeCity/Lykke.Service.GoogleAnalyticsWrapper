using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.GoogleAnalyticsWrapper.AzureRepositories.TrackerUser
{
    public class GaUserEntity: TableEntity, IGaUser
    {
        public string ClientId { get; set; }
        public string TrackerUserId {get;set;}

        internal static string GeneratePartitionKey() => "TrackerUser";
        internal static string GenerateRowKey(string clientId) => clientId;

        public static GaUserEntity Create(IGaUser src)
        {
            return new GaUserEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.ClientId),
                ClientId = src.ClientId,
                TrackerUserId = src.TrackerUserId
            };
        }
    }
}
