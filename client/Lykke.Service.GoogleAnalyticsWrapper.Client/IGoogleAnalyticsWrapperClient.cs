using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Client.AutorestClient.Models;

namespace Lykke.Service.GoogleAnalyticsWrapper.Client
{
    /// <summary>
    /// Client for google analytics wrapper service
    /// </summary>
    public interface IGoogleAnalyticsWrapperClient
    {
        /// <summary>
        /// Gets GaUserId for clientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<string> GetGaUserIdAsync(string clientId);

        /// <summary>
        /// Sends event about user registration to GA
        /// </summary>
        /// <param name="model">track event model</param>
        /// <returns></returns>
        Task UserRegisteredEventAsync(TrackEventModel model);
        
        /// <summary>
        /// Sends event about use kyc completion to GA
        /// </summary>
        /// <param name="model">track event model</param>
        /// <returns></returns>
        Task KycCompletedEventAsync(TrackEventModel model);
        
        /// <summary>
        /// Sends event about margin trading order creaation to GA
        /// </summary>
        /// <param name="model">track event model</param>
        /// <returns></returns>
        Task MtOrderCreatedEventAsync(TrackEventModel model);
        
        /// <summary>
        /// Sends event about deposit/withdraw to GA
        /// </summary>
        /// <param name="model">track event model</param>
        /// <returns></returns>
        Task WithdrawDepositEventAsync(WithdrawDepositEventModel model);
    }
}
