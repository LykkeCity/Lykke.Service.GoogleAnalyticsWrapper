using System.Threading.Tasks;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}