using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Services
{
    public interface IGaTrackerService
    {
        Task SendEvent(TrackerInfo model, string category, string eventName, string eventValue = null);
        Task SendWithdrawDepositEvent(WithdrawDepositInfo model);
        Task SendTransaction(TransactionInfo model);
    }
}
