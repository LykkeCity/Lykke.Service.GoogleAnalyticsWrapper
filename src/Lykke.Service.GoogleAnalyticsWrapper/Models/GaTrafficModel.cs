using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.GoogleAnalyticsWrapper.Models
{
    public class GaTrafficModel
    {
        [Required]
        public string ClientId { get; set; }
        public string Source { get; set; }
        public string Medium { get; set; }
        public string Campaign { get; set; }
        public string Keyword { get; set; }
    }
}
