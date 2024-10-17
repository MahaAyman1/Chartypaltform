namespace Chartypaltform.ViewModels
{
    public class VolunteerListViewModel
    {
        public string UserName { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public List<string> SelectedTasks { get; set; }
        public int Age { get; set; }    
        public string Gender { get; set; }  
        public string Address { get; set; } 
    }

}
