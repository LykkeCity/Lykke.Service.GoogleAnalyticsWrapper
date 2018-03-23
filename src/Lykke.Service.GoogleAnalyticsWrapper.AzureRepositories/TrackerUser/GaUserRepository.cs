using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;

namespace Lykke.Service.GoogleAnalyticsWrapper.AzureRepositories.TrackerUser
{
    public class GaUserRepository : IGaUserRepository
    {
        private readonly INoSQLTableStorage<GaUserEntity> _tableStorage;

        public GaUserRepository(INoSQLTableStorage<GaUserEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task AddAsync(IGaUser client)
        {
            await _tableStorage.InsertOrMergeAsync(GaUserEntity.Create(client));
        }

        public async Task<IGaUser> GetGaUserAsync(string clientId)
        {
            var entity = await _tableStorage.GetDataAsync(GaUserEntity.GeneratePartitionKey(),
                GaUserEntity.GenerateRowKey(clientId));

            return entity != null ? GaUser.Create(entity) : null;
        }

        public async Task<IEnumerable<IGaUser>> GetAllAsync()
        {
            return (await _tableStorage.GetDataAsync(GaUserEntity.GeneratePartitionKey()))
                .Select(GaUser.Create);
        }
    }
}
