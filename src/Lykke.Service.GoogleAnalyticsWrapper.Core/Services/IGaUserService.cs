using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IGaUserService
    {
        Task<string> GetGaUserIdAsync(string clientId);
        Task<GaTraffic> GetGaUserTrafficAsync(string clientId);
        Task AddGaUserTrafficAsync(IGaTraffic traffic);
    }
}
