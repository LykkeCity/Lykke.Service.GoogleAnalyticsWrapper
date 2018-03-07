using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.GoogleAnalyticsWrapper.AzureRepositories.TrackerUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Services;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Settings.ServiceSettings;
using Lykke.Service.GoogleAnalyticsWrapper.Services;
using Lykke.Service.RateCalculator.Client;
using Lykke.SettingsReader;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;

namespace Lykke.Service.GoogleAnalyticsWrapper.Modules
{
    public class ServiceModule : Module
    {
        private readonly IReloadingManager<AppSettings> _allSettings;
        private readonly IReloadingManager<GoogleAnalyticsWrapperSettings> _settings;
        private readonly ILog _log;

        public ServiceModule(IReloadingManager<AppSettings> settings, ILog log)
        {
            _allSettings = settings;
            _settings = settings.Nested(x => x.GoogleAnalyticsWrapperService);
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            builder.Register(c => new RedisCache(new RedisCacheOptions
            {
                Configuration = _settings.CurrentValue.CacheSettings.RedisConfiguration,
                InstanceName = $"{_settings.CurrentValue.CacheSettings.InstanceName}:"
            }))
            .As<IDistributedCache>()
            .SingleInstance();

            builder.RegisterType<GaUserService>()
                .As<IGaUserService>()
                .SingleInstance();
            
            builder.RegisterType<GaTrackerService>()
                .As<IGaTrackerService>()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.TrackAssets))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.GaSettings))
                .WithParameter(TypedParameter.From(_settings.CurrentValue.IsLive))
                .SingleInstance();
            
            builder.RegisterInstance(
                new GaUserRepository(AzureTableStorage<GaUserEntity>.Create(_settings.ConnectionString(x => x.Db.DataConnString), "TrackerUsers", _log))
            ).As<IGaUserRepository>().SingleInstance();
            
            builder.RegisterRateCalculatorClient(_allSettings.CurrentValue.RateCalculatorServiceClient.ServiceUrl, _log);
        }
    }
}
