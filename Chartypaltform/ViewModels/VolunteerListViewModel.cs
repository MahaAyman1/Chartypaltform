using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Chartypaltform.ViewModels
{
    public class VolunteerListViewModel
    {
        public int Id { get; set; } 
        public string UserName { get; set; }
        public DateTime AvailableFrom { get; set; }
        public DateTime AvailableTo { get; set; }
        public List<string> SelectedTasks { get; set; }
        public int Age { get; set; }    
        public string Gender { get; set; }  
        public string Address { get; set; }

        public string SelectedGender { get; set; }
        public string phone { get; set; }
        public string FullName { get; set; }

        public List<SelectListItem> ModelGenderList { get; set; }
    }

}
