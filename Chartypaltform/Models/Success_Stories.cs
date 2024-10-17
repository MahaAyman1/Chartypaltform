namespace Chartypaltform.Models
{
    public class Success_Story
    {
        public int Id { get; set; }
        public string Title { get; set; }    
        public string impact { get; set; }
        public int CharityOrganizationId { get; set; }
        public CharityOrganization CharityOrganization { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
