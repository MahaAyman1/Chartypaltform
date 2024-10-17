using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
	
	public class Complaint
	{
		[Key]
		public int ComplaintId { get; set; }



		[Required]
		[MinLength(10)]
		[DataType(DataType.MultilineText)]  
		public string ComplaintText { get; set; }

		[Required]
		public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;  

		[DataType(DataType.DateTime)]
		public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; } // Use string to store user ID
        public ApplicationUser User { get; set; } // Reference to the user
    }

	// Enum for Complaint Status
	public enum ComplaintStatus
	{
		Pending,    // Default
		Resolved,
		Dismissed
	}
}
