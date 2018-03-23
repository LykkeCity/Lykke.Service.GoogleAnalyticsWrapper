using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class WithdrawDepositEventModel : TrackEventModel
    {
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
    }
}
