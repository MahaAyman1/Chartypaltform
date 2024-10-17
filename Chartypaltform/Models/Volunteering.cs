using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
    public class Volunteering
    {
        [Key]
        public int Id { get; set; }

        public DateTime AvailableFrom { get; set; } // When the donor is available to start volunteering

        public DateTime AvailableTo { get; set; } // When the donor is available until
        public string UserId { get; set; } // Use string to store user ID
        public ApplicationUser User { get; set; } // Reference to the user


        public ICollection<VolunteeringTaskSelection> TaskSelections { get; set; } = new List<VolunteeringTaskSelection>();

    }

    public enum VolunteeringTask
    {
        EventOrganizer,
        MentalHealthSupport,
        CommunityOutreach,
        SocialMediaSupport,
        AdministrativeSupport,
        Tutoring,
        Other // For any tasks not specified
    }

}
