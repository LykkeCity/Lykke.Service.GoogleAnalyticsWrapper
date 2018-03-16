using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IGaTrackerService
    {
        Task SendEventAsync(TrackerInfo model, string category, string eventName, string eventValue = null);
        Task SendWithdrawDepositEventAsync(WithdrawDepositInfo model);
        Task SendTransactionAsync(TransactionInfo model);
    }
}
