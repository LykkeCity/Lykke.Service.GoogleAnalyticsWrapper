using System.Threading.Tasks;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic
{
    public interface IGaTrafficRepository
    {
        Task AddAsync(IGaTraffic model);
        Task<IGaTraffic> GetAsync(string clientId);
    }
}
