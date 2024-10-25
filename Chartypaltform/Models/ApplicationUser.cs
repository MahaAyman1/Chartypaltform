using Microsoft.AspNetCore.Identity;

namespace Chartypaltform.Models
{
    public abstract class ApplicationUser : IdentityUser
    {
        public DateTime RegisteredAt { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Img { get; set; }

        public virtual ICollection<Event> CreatedEvents { get; set; } // Events created by the user
        public virtual ICollection<Event> JoinedEvents { get; set; } 

    }

}
