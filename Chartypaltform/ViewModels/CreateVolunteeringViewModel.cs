using Chartypaltform.Models;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class CreateVolunteeringViewModel
    {
        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

        // List of volunteering tasks
        public List<VolunteeringTask> SelectedTasks { get; set; } = new List<VolunteeringTask>();
    }

}
