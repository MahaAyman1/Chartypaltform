using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
namespace Chartypaltform.ViewModels
{
    public class CampaignViewModel
    {
        public string ?CampaignImg { get; set; }

        [Required]
        [MaxLength(100)]
        public string CampaignName { get; set; }

        [Required]
        [MaxLength(500)]
        public string CampaignDes { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal GoalAmount { get; set; }
        public IFormFile? formFile { get; set; }


        public List<int> SelectedCategoryIds { get; set; } = new List<int>();

        
        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>(); 
    }
}
