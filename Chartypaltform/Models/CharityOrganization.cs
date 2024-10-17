namespace Chartypaltform.Models
{
    public class CharityOrganization : ApplicationUser
    {
        public int CharityOrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public RegistrationStatus registration_status { get; set; }
        public string SuccessStories { get; set; }
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
