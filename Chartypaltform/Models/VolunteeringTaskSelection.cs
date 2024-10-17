using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.Models
{
    public class VolunteeringTaskSelection
    {
        [Key]
        public int Id { get; set; }

        // Foreign key to Volunteering
        public int VolunteeringId { get; set; }
        public Volunteering Volunteering { get; set; } // Navigation property

        // The task selected from the enum
        public VolunteeringTask TaskDescription { get; set; }
    }

}
