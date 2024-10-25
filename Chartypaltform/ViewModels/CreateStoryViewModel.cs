using Chartypaltform.Models;

namespace Chartypaltform.ViewModels
{
    public class CreateStoryViewModel
    {
        public List<Campaign> CompletedCampaigns { get; set; }
        public int SelectedCampaignId { get; set; }
        public string Impact { get; set; }
        public string Title { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public IFormFileCollection ImageFiles { get; set; }
        public IFormFile PdfFile { get; set; }
    }

}
