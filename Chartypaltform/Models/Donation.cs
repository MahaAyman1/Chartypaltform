using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
    public class Donation
    {
        [Key]
        public int DonationId { get; set; }

        [Required]
        public decimal Amount { get; set; }
        public string Status { get; set; } = "Pending";

        public DateTime DonationDate { get; set; } = DateTime.Now;

        public string DonorId { get; set; }
        public int CampaignId { get; set; }

        [ForeignKey("DonorId")]
        public virtual Donor Donor { get; set; }

        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
    }
}
