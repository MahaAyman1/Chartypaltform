using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.Service
{
    public class CampaignService
    {
        private readonly ApplicationDbContext _context;

        public CampaignService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetCompletedCampaignCountAsync()
        {
            return await _context.Campaigns
                .CountAsync(c => c.Status == CampaignStatus.Completed);
        }
    }
}
