using System;
using Autofac;

namespace Lykke.Service.GoogleAnalyticsWrapper.Client
{
    /// <summary>
    /// </summary>
    public static class AutofacExtension
    {
        /// <summary>
        /// Registers GoogleAnalyticsWrapperClient as IGoogleAnalyticsWrapperClient
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="serviceUrl"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void RegisterGoogleAnalyticsWrapperClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));
            if (serviceUrl == null) throw new ArgumentNullException(nameof(serviceUrl));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.RegisterType<GoogleAnalyticsWrapperClient>()
                .WithParameter("serviceUrl", serviceUrl)
                .As<IGoogleAnalyticsWrapperClient>()
                .SingleInstance();
        }

        /// <summary>
        /// Registers GoogleAnalyticsWrapperClient as IGoogleAnalyticsWrapperClient
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="settings"></param>
        public static void RegisterGoogleAnalyticsWrapperClient(this ContainerBuilder builder, GoogleAnalyticsWrapperServiceClientSettings settings)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            builder.RegisterGoogleAnalyticsWrapperClient(settings.ServiceUrl);
        }
    }
}
