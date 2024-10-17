using Chartypaltform.Models;

namespace Chartypaltform.ViewModels
{
    public class CharityOrganizationRegistrationModel
    {
        public string OrganizationName { get; set; }
        public RegistrationStatus registration_status { get; set; }
      
 
        public string InstagramLink { get; set; }
        public string FacebookLink { get; set; }
        public string GoogleLink { get; set; }
        public DateTime? CreatedAT { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Img { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

    }
}
