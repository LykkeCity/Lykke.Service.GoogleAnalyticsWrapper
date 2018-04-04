using System;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaUser
{
    public class GaUser : IGaUser
    {
        public string ClientId { get; set; }
        public string TrackerUserId { get; set; }
        public string Cid { get; set; }

        public static GaUser Create(IGaUser src)
        {
            return new GaUser
            {
                ClientId = src.ClientId,
                TrackerUserId = src.TrackerUserId,
                Cid = src.Cid
            };
        }

        public static GaUser CreateNew(string clientId)
        {
            return new GaUser
            {
                ClientId = clientId,
                TrackerUserId = GenerateNewUserId(),
                Cid = GenerateNewCid()
            };
        }

        public static string GenerateNewUserId()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateNewCid()
        {
            var rand = new Random(DateTime.UtcNow.Millisecond);
            return $"{Get9DigitNum()}.{Get9DigitNum()}";
            
            string Get9DigitNum()
            {
                return rand.Next(999999999).ToString(new string('0', 9));
            }
        }
    }
}
