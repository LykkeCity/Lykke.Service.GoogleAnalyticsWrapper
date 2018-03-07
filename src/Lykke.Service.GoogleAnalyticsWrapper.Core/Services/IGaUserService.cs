using System.Threading.Tasks;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IGaUserService
    {
        Task<string> GetGaUserIdAsync(string clientId);
    }
}
