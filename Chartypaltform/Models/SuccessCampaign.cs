namespace Chartypaltform.Models
{
    public class SuccessCampaign
    {
        public int Id { get; set; }
        public int CampaignId { get; set; }
        public Campaign Campaign { get; set; }

        public string impact {  get; set; } 
        public string title { get; set; }

        public List<string> ImagePaths { get; set; }
        public string PdfPath { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;


    }
}
