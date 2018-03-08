using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Common.Log;
using Flurl.Http;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker;
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
        private readonly ILog _log;

        public GaTrackerService(
            IGaUserService gaUserService,
            IRateCalculatorClient rateCalculatorClient,
            TrackAssetsSttings trackAssetsSttings,
            GaSettings gaSettings,
            bool isLive,
            ILog log
            )
        {
            _gaUserService = gaUserService;
            _rateCalculatorClient = rateCalculatorClient;
            _trackAssetsSttings = trackAssetsSttings;
            _gaSettings = gaSettings;
            _isLive = isLive;
            _log = log;
        }

        public async Task SendEvent(TrackerInfo model, string category, string eventName, string eventValue = null)
        {
            var eventInfo = EventInfo.Create(model);
            eventInfo.EventCategory = GetCategoryName(category);
            eventInfo.EventName = eventName;
            eventInfo.EventValue = eventValue;
            await SendEventAsync(eventInfo);
        }

        public async Task SendWithdrawDepositEvent(WithdrawDepositInfo model)
        {
            BaseAssetSettings baseAsset = GetBaseAsset(model.AssetId);

            if (baseAsset == null)
            {
                _log.WriteWarning(nameof(SendWithdrawDepositEvent), model.UserId, $"{model.AssetId} is not tracked. (asset is not in Fiat or Crypto lists)");
                return;
            }

            var assetAmount = await GetAmountAsync(model.AssetId, model.Amount, baseAsset);

            if (assetAmount == 0)
            {
                _log.WriteWarning(nameof(SendWithdrawDepositEvent), model.UserId, $"Can't convert {model.Amount} {model.AssetId} to {baseAsset.AssetId} (Multiplier = {baseAsset.Multiplier})");
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
                UserId = model.UserAgent
            };

            await SendEvent(trackerInfo, TrackerCategories.Wallet, walletEventName);
            await SendEvent(trackerInfo, TrackerCategories.Wallet, eventName, Math.Round(Math.Abs(assetAmount * baseAsset.Multiplier)).ToString(CultureInfo.InvariantCulture));
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
            var baseAssetAmount = await GetAmountAsync(assetId, amount, _trackAssetsSttings.Fiat.BaseAsset);

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
        
        private async Task<double> GetAmountAsync(string assetId, double amount, BaseAssetSettings baseAsset)
        {
            return assetId == baseAsset.AssetId 
                ? amount 
                : await _rateCalculatorClient.GetAmountInBaseAsync(assetId, amount, baseAsset.AssetId);
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

        private async Task SendEventAsync(EventInfo model)
        {
            await FillEventInfoModel(model);
            
            var gaEvent = GaEvent.Create(model, _gaSettings.ApiKey, model.EventCategory, model.EventName, model.EventValue, "start");
            await SendDataAsync(gaEvent);

            var gaPageView = GaPageView.Create(model, _gaSettings.ApiKey, model.EventCategory, model.EventName, "end");
            await SendDataAsync(gaPageView);
        }
        
        private async Task FillEventInfoModel(EventInfo model)
        {
            string gaUserId = await _gaUserService.GetGaUserIdAsync(model.ClientId);
            model.ClientId = gaUserId;
            var deviceInfo = new DeviceInfo();

            deviceInfo.ParseUserAgent(model.UserAgent);
            deviceInfo.ParseClientInfo(model.ClientInfo);

            model.UserAgent = deviceInfo.GetUserAgentString();
            model.ScreenResolution = deviceInfo.ScreenResolution;
            model.AppVersion = deviceInfo.AppVersion;
        }
        
        private async Task SendDataAsync<T>(T model) where T: GaBaseEvent
        {
            var data = model.Transform();

            if (model is GaEvent)
            {
                var gaEvent = model as GaEvent;

                _log.WriteInfo(nameof(SendDataAsync), gaEvent, "sending request to tracker");
            }

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
