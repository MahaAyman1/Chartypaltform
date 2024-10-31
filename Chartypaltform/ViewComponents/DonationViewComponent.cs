using Chartypaltform.Data;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.ViewComponents
{
    public class DonationViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;
        public DonationViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var donations = await _db.donations
                .Include(d => d.Donor)
                .Include(d => d.Campaign)
                .OrderByDescending(d => d.DonationDate) 
                .Take(9) 
                .Select(d => new DonationViewModel
                {
                    DonationId = d.DonationId,
                    Amount = d.Amount,
                    DonorName = d.Donor.FullName,
                    CampaignName = d.Campaign.CampaignName,
                    DonationDate = d.DonationDate,
                    img = d.Donor.Img,
                })
                .ToListAsync();

            return View(donations);
        }

    }
}
