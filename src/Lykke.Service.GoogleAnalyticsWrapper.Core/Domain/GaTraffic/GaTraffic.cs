using System;
using Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTraffic
{
    public class GaTraffic : IGaTraffic
    {
        public string ClientId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string Keyword { get; set; }
        public string Content { get; set; }

        public static GaTraffic CreateDefault(string clientId)
        {
            return new GaTraffic
            {
                ClientId = clientId,
                Source = GaParamValue.Direct,
                Medium = GaParamValue.None,
                Campaign = GaParamValue.None,
                Keyword = GaParamValue.None,
                Content = GaParamValue.None
            };
        }

        public static GaTraffic Parse(string clientId, string traffic)
        {
            if (string.IsNullOrEmpty(traffic))
                return null;
            
            var values = traffic.Split("|||");

            if (values == null || values.Length <= 0)
                return null;
            
            var model = new GaTraffic {ClientId = clientId};

            foreach (var value in values)
            {
                var item = value.Split(new[] {"="}, StringSplitOptions.RemoveEmptyEntries);

                if (item.Length != 2) 
                    continue;
                
                switch (item[0])
                {
                    case "src":
                        model.Source = item[1];
                        break;
                    case "mdm":
                        model.Medium = item[1];
                        break;
                    case "cmp":
                        model.Campaign = item[1];
                        break;
                    case "trm":
                        model.Keyword = item[1];
                        break;
                    case "cnt":
                        model.Content = item[1];
                        break;
                }
            }

            return model;
        }
    }
}
