namespace Chartypaltform.Models
{
    public class NavItem
    {
        public int Id { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
        public string Visibility { get; set; } // "Authenticated", "Anonymous", or "Both"
  

    }
}
