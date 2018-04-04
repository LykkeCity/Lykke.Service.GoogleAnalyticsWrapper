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
        /// Gets traffic information for clientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        Task<GaTraffic> GetGaUserTrafficAsync(string clientId);
        
        /// <summary>
        /// Adds traffic information for clientId
        /// </summary>
        /// <param name="model">traffic info</param>
        /// <returns></returns>
        Task AddGaUserTrafficAsync(GaTrafficModel model);
        
        /// <summary>
        /// Adds GA cid for clientId
        /// </summary>
        /// <param name="model">cid model</param>
        /// <returns></returns>
        Task AddGaCidAsync(GaUserModel model);

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
        
        /// <summary>
        /// Sends cash out fee as a transaction to GA
        /// </summary>
        /// <param name="model">cash out fee model</param>
        /// <returns></returns>
        Task TrackCashoutAsync(TransactionModel model);
        
        /// <summary>
        /// Sends trade fee as a transaction to GA
        /// </summary>
        /// <param name="model">trade fee model</param>
        /// <returns></returns>
        Task TrackTradeAsync(TransactionModel model);
    }
}
