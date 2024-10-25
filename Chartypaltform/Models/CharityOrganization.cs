using System.ComponentModel.DataAnnotations.Schema;

namespace Chartypaltform.Models
{
    public class CharityOrganization : ApplicationUser
    {
        
        public string OrganizationName { get; set; }
        public RegistrationStatus registration_status { get; set; }
        public string Event { get; set; }
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string GoogleLink { get; set; }
        public DateTime? CreatedAT { get; set; }
    }
    public enum RegistrationStatus
    {
        Pending,
        Approved,
        Rejected
    }


}
