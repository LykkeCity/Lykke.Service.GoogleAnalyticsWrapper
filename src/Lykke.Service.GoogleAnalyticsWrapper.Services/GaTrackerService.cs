using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Common.Log;
using Flurl.Http;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ServiceSettings;
using Lykke.Service.RateCalculator.Client;

namespace Lykke.Service.GoogleAnalyticsWrapper.Services
{
    public class GaTrackerService : IGaTrackerService
    {
        private readonly IGaUserService _gaUserService;
        private readonly IRateCalculatorClient _rateCalculatorClient;
        private readonly TrackAssetsSttings _trackAssetsSttings;
        private readonly GaSettings _gaSettings;
        private readonly bool _isLive;
        private readonly string _transactionAssetId;
        private readonly ILog _log;

        public GaTrackerService(
            IGaUserService gaUserService,
            IRateCalculatorClient rateCalculatorClient,
            TrackAssetsSttings trackAssetsSttings,
            GaSettings gaSettings,
            bool isLive,
            string transactionAssetId,
            ILog log
            )
        {
            _gaUserService = gaUserService;
            _rateCalculatorClient = rateCalculatorClient;
            _trackAssetsSttings = trackAssetsSttings;
            _gaSettings = gaSettings;
            _isLive = isLive;
            _transactionAssetId = transactionAssetId;
            _log = log.CreateComponentScope(nameof(GaTrackerService));
        }

        public async Task SendEventAsync(TrackerInfo model, string category, string eventName, string eventValue = null)
        {
            var eventCategory = GetCategoryName(category);
            var gaEvent = GaEvent.Create(model, eventCategory, eventName, eventValue);
            
            await FillGaHitAsync(gaEvent);
            await SendDataAsync(gaEvent);
        }

        public async Task SendWithdrawDepositEventAsync(WithdrawDepositInfo model)
        {
            BaseAssetSettings baseAsset = GetBaseAsset(model.AssetId);

            if (baseAsset == null)
            {
                _log.WriteWarning(nameof(SendWithdrawDepositEventAsync), model.UserId, $"{model.AssetId} is not tracked. (asset is not in Fiat or Crypto lists)");
                return;
            }

            var assetAmount = await GetAmountAsync(model.AssetId, model.Amount, baseAsset.AssetId);

            if (assetAmount == 0)
            {
                _log.WriteWarning(nameof(SendWithdrawDepositEventAsync), model.UserId, $"Can't convert {model.Amount} {model.AssetId} to {baseAsset.AssetId} (Multiplier = {baseAsset.Multiplier})");
                return;
            }
            
            var walletEventName = await GetWalletEventNameAsync(assetAmount, model.AssetId);
            var eventName = GetDepositWithdrawEventName(assetAmount, model.AssetId, baseAsset.Multiplier);

            if (eventName == null)
                return;

            var trackerInfo = new TrackerInfo
            {
                Ip = model.Ip,
                UserAgent = model.UserAgent,
                UserId = model.UserId,
                ClientInfo = model.ClientInfo
            };

            await SendEventAsync(trackerInfo, TrackerCategories.Wallet, walletEventName);
            await SendEventAsync(trackerInfo, TrackerCategories.Wallet, eventName, Math.Round(Math.Abs(assetAmount * baseAsset.Multiplier)).ToString(CultureInfo.InvariantCulture));
        }

        public async Task SendTransactionAsync(TransactionInfo model)
        {
            var amount = await GetAmountAsync(model.AssetId, model.Amount, _transactionAssetId);

            if (amount == 0)
            {
                _log.WriteWarning(nameof(SendTransactionAsync), model.UserId, $"Can't convert {model.Amount} {model.AssetId} to {_transactionAssetId}");
                return;
            }
            
            var transaction = GaTransaction.Create(model, Math.Abs(amount), _transactionAssetId);
            await FillGaHitAsync(transaction);
            await SendDataAsync(transaction);
            
            var item = GaItem.Create(transaction);
            await SendDataAsync(item);
        }

        private BaseAssetSettings GetBaseAsset(string assetId)
        {
            if (_trackAssetsSttings.Fiat.Assets.Contains(assetId))
                return _trackAssetsSttings.Fiat.BaseAsset;

            if (_trackAssetsSttings.Crypto.Assets.Contains(assetId))
                return _trackAssetsSttings.Crypto.BaseAsset;

            return null;
        }
        
        private string GetCategoryName(string categoryName)
        {
            return _isLive ? categoryName : TrackerCategories.Sandbox;
        }
        
        private async Task<string> GetWalletEventNameAsync(double amount, string assetId)
        {
            var baseAssetAmount = await GetAmountAsync(assetId, amount, _trackAssetsSttings.Fiat.BaseAsset.AssetId);

            bool isDeposit = amount > 0;

            switch (Math.Abs(baseAssetAmount))
            {
                case var a when a <= 100:
                    return isDeposit ? TrackerEvents.DslotA : TrackerEvents.WslotA;
                case var a when a > 100 && a <= 1000:
                    return isDeposit ? TrackerEvents.DslotB : TrackerEvents.WslotB;
                default:
                    return isDeposit ? TrackerEvents.DslotC : TrackerEvents.WslotC;
            }
        }
        
        private async Task<double> GetAmountAsync(string assetId, double amount, string baseAssetId)
        {
            return assetId == baseAssetId 
                ? amount 
                : await _rateCalculatorClient.GetAmountInBaseAsync(assetId, amount, baseAssetId);
        }
        
        private string GetDepositWithdrawEventName(double amount, string assetId, int multiplier)
        {
            BaseAssetSettings baseAsset = null;
            string assetType = null;

            if (_trackAssetsSttings.Fiat.Assets.Contains(assetId))
            {
                assetType = "Fiat";
                baseAsset = _trackAssetsSttings.Fiat.BaseAsset;
            }

            if (_trackAssetsSttings.Crypto.Assets.Contains(assetId))
            {
                assetType = "Crypto";
                baseAsset = _trackAssetsSttings.Crypto.BaseAsset;
            }

            return !string.IsNullOrEmpty(assetType) 
                ? $"{(amount > 0 ? "Deposit" : "Withdraw")}{assetType}{(baseAsset.Multiplier == 1 ? string.Empty : $"_{multiplier:g2}")}_{baseAsset.AssetId}" 
                : null;
        }
        
        private async Task FillGaHitAsync(GaBaseHit model)
        {
            GaUser gaUser = await _gaUserService.GetGaUserAsync(model.UserId, model.Cid);
            model.UserId = gaUser.TrackerUserId;
            model.Cid = gaUser.Cid;
            model.TrackingId = _gaSettings.ApiKey;
            
            model.Traffic = await _gaUserService.GetGaUserTrafficAsync(gaUser.ClientId);
            
            var deviceInfo = new DeviceInfo();

            deviceInfo.ParseUserAgent(model.UserAgent);
            deviceInfo.ParseClientInfo(model.ClientInfo);

            model.UserAgent = deviceInfo.GetUserAgentString();
            model.ScreenResolution = deviceInfo.ScreenResolution;
            model.AppVersion = deviceInfo.AppVersion;
        }
        
        private async Task SendDataAsync(GaBaseHit model)
        {
            var data = model.Transform();

            _log.WriteInfo(nameof(SendDataAsync), data, model.Type);

            HttpResponseMessage response = await _gaSettings.ApiUrl
                .WithHeader("User-Agent", model.UserAgent)
                .PostUrlEncodedAsync(data);

            if (!response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadAsStringAsync();
                _log.WriteWarning(nameof(SendEventAsync), model, $"{_gaSettings.ApiUrl} returned {response.StatusCode}: {res}");
            }
        }
    }
}
