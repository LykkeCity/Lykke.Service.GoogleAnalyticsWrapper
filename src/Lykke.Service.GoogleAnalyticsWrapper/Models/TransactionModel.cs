using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class TransactionModel : TrackEventModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public string AssetId { get; set; }
    }
}
