using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
	public class Campaign
	{
		[Key]
		public int CampaignId { get; set; }

		[Required]
		public string CampaignImg { get; set; }

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

		// Foreign Key Relationship to Category
		public int CategoryId { get; set; }
		public List<Category> Categories { get; set; }
		public string UserId { get; set; } // Use string to store user ID
		public ApplicationUser User { get; set; } // Reference to the user
	}

	// Enum for Campaign Status
	public enum CampaignStatus
	{
		Pending, // Default
		Active,
		Completed,
		Closed
	}
}





