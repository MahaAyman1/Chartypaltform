using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chartypaltform.Models
{
    public class Event
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Event Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Location Address")]
        public string Location { get; set; } 

        [Required]
        [Display(Name = "Google Maps Link")]
        public string LocationUrl { get; set; } 
        [Required]
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Event Date")]
        public DateTime EventDate { get; set; }

        [Display(Name = "Event Image URL")]
        public string ?ImgUrl { get; set; } 

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Max Number of Participants")]
        public int MaxParticipants { get; set; }

        [NotMapped]
        public IFormFile? formFile { get; set; }

        

        public string OrganizationId { get; set; }
       
        // List of donors who joined
        public virtual ICollection<Donor> Attendees { get; set; } = new List<Donor>();
    }
}
