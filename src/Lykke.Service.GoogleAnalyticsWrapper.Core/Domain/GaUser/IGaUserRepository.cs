using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser
{
    public interface IGaUserRepository
    {
        Task AddAsync(IGaUser client);
        Task<IGaUser> GetGaUserAsync(string clientId);
        Task<IEnumerable<IGaUser>> GetAllAsync();
    }
}
