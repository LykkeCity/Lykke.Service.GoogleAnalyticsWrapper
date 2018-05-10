using Common;

namespace Lykke.Service.GoogleAnalyticsWrapper.Core.Domain.GaTracker
{
    public class GaItem : GaTransaction
    {
        public static GaItem Create(GaTransaction transaction)
        {
            return new GaItem
            {
                TrackingId = transaction.TrackingId,
                Type = GaHitType.GaItem,
                Cid = transaction.Cid,
                UserId = transaction.UserId,
                UserAgent = transaction.UserAgent,
                ClientInfo = transaction.ClientInfo,
                ScreenResolution = transaction.ScreenResolution,
                AppVersion = transaction.AppVersion,
                Ip = transaction.Ip.SanitizeIp(),
                Id = transaction.Id,
                Amount = transaction.Amount,
                AssetId = transaction.AssetId,
                Name = transaction.Name,
                Traffic = transaction.Traffic
            };
        }
        
        public override object Transform()
        {
            return new
            {
                v = Version,
                tid = TrackingId,
                t = Type,
                cid = Cid,
                uid = UserId,
                cn = Traffic?.Campaign,
                cs = Traffic?.Source,
                cm = Traffic?.Medium,
                ck = Traffic?.Keyword,
                ti = Id,
                @in = Name,
                ip = Amount,
                cu = AssetId,
                iq = 1,
                ic = GaParamValue.ItemCategory,
                uip = Ip,
                ua = UserAgent
            };
        }
    }
}
