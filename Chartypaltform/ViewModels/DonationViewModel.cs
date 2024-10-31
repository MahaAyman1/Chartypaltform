namespace Chartypaltform.ViewModels
{
    public class DonationViewModel
    {
        public int DonationId { get; set; }
        public decimal Amount { get; set; }
        public string DonorName { get; set; }
        public string CampaignName { get; set; }
        public DateTime DonationDate { get; set; }

        public string img {  get; set; }    
    }
}
