using Chartypaltform.Models;
using System.ComponentModel.DataAnnotations;

namespace Chartypaltform.ViewModels
{
    public class CreateVolunteeringViewModel
    {
        public int Id { get; set; }  // Add this line

        [Required]
        public DateTime AvailableFrom { get; set; }

        [Required]
        public DateTime AvailableTo { get; set; }

         public List<VolunteeringTask> SelectedTasks { get; set; } = new List<VolunteeringTask>();

    }

}
