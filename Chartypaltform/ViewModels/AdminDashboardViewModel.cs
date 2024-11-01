namespace Chartypaltform.ViewModels
{
    public class AdminDashboardViewModel
    {
        public string AdminFullName { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
		public int OpenCampaignsCount { get; set; } = 0;
		public int ApprovedOrganizationsCount { get; set; }
		public int RegisteredUserCount { get; set; }
		public double WeeklyUserGrowthRate { get; set; }

	}
}
