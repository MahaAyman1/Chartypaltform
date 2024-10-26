using System.ComponentModel.DataAnnotations.Schema;

namespace Chartypaltform.Models
{
    public class Donor : ApplicationUser
    {

        public string FullName { get; set; }
      
        public int Age { get; set; }
        public string Gender { get; set; }
        public virtual ICollection<Donation> Donations { get; set; }


    }

}
