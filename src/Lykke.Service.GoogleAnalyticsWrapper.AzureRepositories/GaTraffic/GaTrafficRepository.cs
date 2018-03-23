using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;

namespace Lykke.Service.GoogleAnalyticsWrapper.AzureRepositories.GaTraffic
{
    public class GaTrafficRepository : IGaTrafficRepository
    {
        private readonly INoSQLTableStorage<GaTrafficEntity> _tableStorage;

        public GaTrafficRepository(INoSQLTableStorage<GaTrafficEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }
        
        public  Task AddAsync(IGaTraffic model)
        {
            var entity = GaTrafficEntity.Create(model);
            return _tableStorage.InsertOrMergeAsync(entity);
        }

        public async Task<IGaTraffic> GetAsync(string clientId)
        {
            return await _tableStorage.GetDataAsync(GaTrafficEntity.GeneratePartitionKey(), GaTrafficEntity.GenerateRowKey(clientId));
        }
    }
}
