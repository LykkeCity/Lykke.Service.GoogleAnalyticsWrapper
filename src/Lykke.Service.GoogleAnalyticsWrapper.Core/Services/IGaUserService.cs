using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IGaUserService
    {
        Task<GaUser> GetGaUserAsync(string clientId, string cid = null);
        Task<GaTraffic> GetGaUserTrafficAsync(string clientId);
        Task AddGaUserTrafficAsync(IGaTraffic traffic);
    }
}
