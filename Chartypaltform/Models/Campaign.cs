using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chartypaltform.Models
{
	public class Campaign
	{
		[Key]
		public int CampaignId { get; set; }
        [Required]

        public string? CampaignImg { get; set; }
        [NotMapped]
        public IFormFile? formFile { get; set; }
        [Required]
		[MaxLength(100)]
		public string CampaignName { get; set; }

		[Required]
		[MaxLength(500)]
		public string CampaignDes { get; set; }

		[Required]
		[Range(0, double.MaxValue)]
		public decimal GoalAmount { get; set; }

		[Range(0, double.MaxValue)]
		public decimal CurrentAmountRaised { get; set; }

		[Required]
		public CampaignStatus Status { get; set; } = CampaignStatus.Pending;

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		public int CategoryId { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual CharityOrganization User { get; set; }
		public ICollection<SuccessCampaign> SuccessCampaigns { get; set; }

        public virtual ICollection<Donation> Donations { get; set; }


    }


    public enum CampaignStatus
	{
		Pending, // Default
		Active,
		Completed,
		Closed
	}
}