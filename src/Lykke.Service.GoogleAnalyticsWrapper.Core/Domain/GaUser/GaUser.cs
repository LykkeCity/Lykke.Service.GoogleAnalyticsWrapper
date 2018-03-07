namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser
{
    public class GaUser : IGaUser
    {
        public string ClientId { get; set; }
        public string TrackerUserId { get; set; }

        public static GaUser Create(IGaUser src)
        {
            return new GaUser
            {
                ClientId = src.ClientId,
                TrackerUserId = src.TrackerUserId
            };
        }
    }
}
