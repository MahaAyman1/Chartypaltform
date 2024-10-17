namespace Chartypaltform.Models
{
    public class Donor : ApplicationUser
    {
        public int Id { get; set; }  // Primary key

        public string FullName { get; set; }
      
        public int Age { get; set; }
        public string Gender { get; set; }
      
    }

}
