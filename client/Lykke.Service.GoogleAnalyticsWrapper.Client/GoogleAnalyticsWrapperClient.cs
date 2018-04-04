using System;
using System.Threading.Tasks;
using Lykke.Service.GoogleAnalyticsWrapper.Client.AutorestClient;
using Lykke.Service.GoogleAnalyticsWrapper.Client.AutorestClient.Models;

namespace Lykke.Service.GoogleAnalyticsWrapper.Client
{
    /// <inheritdoc cref="IGoogleAnalyticsWrapperClient"/>
    public class GoogleAnalyticsWrapperClient : IGoogleAnalyticsWrapperClient, IDisposable
    {
        private GoogleAnalyticsWrapperAPI _service;
        private const string TechnicalProblem = "Technical problem";

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceUrl"></param>
        public GoogleAnalyticsWrapperClient(string serviceUrl)
        {
            _service = new GoogleAnalyticsWrapperAPI(new Uri(serviceUrl));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_service == null)
                return;
            _service.Dispose();
            _service = null;
        }

        /// <inheritdoc />
        public async Task<string> GetGaUserIdAsync(string clientId)
        {
            var response = await _service.GetGaUserIdAsync(clientId);
            
            switch (response)
            {
                case string result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            throw new Exception(TechnicalProblem);
        }

        /// <inheritdoc />
        public async Task<GaTraffic> GetGaUserTrafficAsync(string clientId)
        {
            var response = await _service.GetGaUserTrafficAsync(clientId);

            if (response == null)
                return null;
            
            switch (response)
            {
                case GaTraffic result:
                    return result;
                case ErrorResponse error:
                    throw new Exception(error.ErrorMessage);
            }

            throw new Exception(TechnicalProblem);
        }

        /// <inheritdoc />
        public async Task AddGaUserTrafficAsync(GaTrafficModel model)
        {
            var response = await _service.AddGaUserTrafficAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task AddGaCidAsync(GaUserModel model)
        {
            var response = await _service.AddGaUserCidAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task UserRegisteredEventAsync(TrackEventModel model)
        {
            var response = await _service.UserRegisteredEventAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task KycCompletedEventAsync(TrackEventModel model)
        {
            var response = await _service.KycCompletedEventAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task MtOrderCreatedEventAsync(TrackEventModel model)
        {
            var response = await _service.MtOrderCreatedEventAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task WithdrawDepositEventAsync(WithdrawDepositEventModel model)
        {
            var response = await _service.WithdrawDepositEventAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task TrackCashoutAsync(TransactionModel model)
        {
            var response = await _service.TrackCashoutAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }

        /// <inheritdoc />
        public async Task TrackTradeAsync(TransactionModel model)
        {
            var response = await _service.TrackTradeAsync(model);

            if (response != null)
                throw new Exception(response.ErrorMessage);
        }
    }
}
