using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace Chartypaltform.ViewComponents
{
    public class ListOfCampaignViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ListOfCampaignViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var campaigns = await _db.Campaigns
                .Include(c => c.User)
                .Where(c => c.Status == CampaignStatus.Active)
                .ToListAsync();

            foreach (var campaign in campaigns)
            {
                if (campaign.CurrentAmountRaised >= campaign.GoalAmount)
                {
                    campaign.Status = CampaignStatus.Completed;
                    _db.Campaigns.Update(campaign);
                }
            }

            // Save changes after updating campaigns
            await _db.SaveChangesAsync();

            return View(campaigns);
        }
    }
}
