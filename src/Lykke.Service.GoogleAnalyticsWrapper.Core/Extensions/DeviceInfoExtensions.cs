using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Extensions
{
    public static class DeviceInfoExtensions
    {
        public static void ParseClientInfo(this DeviceInfo deviceInfo, string clientInfo)
        {
            if (!string.IsNullOrEmpty(clientInfo))
            {
                var values = clientInfo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                if (values.Length != 4)
                    return;

                deviceInfo.DeviceType = GetPairValue(values[0]);
                deviceInfo.DeviceModel = GetPairValue(values[1]);

                if (deviceInfo.DeviceType != "android")
                    deviceInfo.OsVersion = GetPairValue(values[2]);

                deviceInfo.ScreenResolution = GetPairValue(values[3]);
            }
        }

        public static void ParseUserAgent(this DeviceInfo deviceInfo, string userAgent)
        {
            var values = ParseUserAgent(userAgent);

            if (!string.IsNullOrEmpty(userAgent) && values.Keys.Count == 0)
            {
                deviceInfo.RawUserAgent = userAgent;
                return;
            }

            if (values.ContainsKey("DeviceType"))
                deviceInfo.DeviceType = values["DeviceType"];

            if (values.ContainsKey("DeviceModel"))
                deviceInfo.DeviceModel = values["DeviceModel"];

            if (values.ContainsKey("AndroidVersion"))
                deviceInfo.OsVersion = values["AndroidVersion"];

            if (values.ContainsKey("AppVersion"))
                deviceInfo.AppVersion = values["AppVersion"];

            deviceInfo.Os = deviceInfo.DeviceType == "android" ? "android" : "iOS";
        }

        public static string GetUserAgentString(this DeviceInfo deviceInfo)
        {
            if (!string.IsNullOrEmpty(deviceInfo.RawUserAgent))
                return deviceInfo.RawUserAgent;

            switch (deviceInfo.Os)
            {
                case "android":
                    return $"Mozilla/5.0 (Linux; Android {deviceInfo.OsVersion}; {deviceInfo.DeviceModel} Build/GNRTD)";
                case "iOS":
                    return $"Mozilla/5.0 ({deviceInfo.DeviceType}; CPU OS {deviceInfo.OsVersion?.Replace('.', '_')} like Mac OS X)";
            }

            return string.Empty;
        }

        private static string GetPairValue(string pair)
        {
            var values = pair.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

            var result = values.Length == 2 
                ? values[1] 
                : values[0];

            return GetCleanValue(result);
        }

        private static string GetCleanValue(string value)
        {
            return value?.Replace("<", string.Empty).Replace(">", string.Empty);
        }

        private static IDictionary<string, string> ParseUserAgent(string userAgent)
        {
            if (!string.IsNullOrEmpty(userAgent))
                return userAgent.Split(';').Select(parameter => parameter.Split('='))
                    .Where(x => x.Length == 2)
                    .GroupBy(x => x[0])
                    .ToDictionary(x => x.Key, x => x.First()[1]);
            return new Dictionary<string, string>();
        }
    }
}
