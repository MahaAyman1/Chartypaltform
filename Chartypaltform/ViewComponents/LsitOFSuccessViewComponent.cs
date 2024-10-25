using Chartypaltform.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.ViewComponents
{
    public class LsitOFSuccessViewComponent :ViewComponent
    {
        private readonly ApplicationDbContext _db; 
      public LsitOFSuccessViewComponent (ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var successCampaigns = _db.successCampaigns
              .Include(sc => sc.Campaign.User)
              .ToList();
            return View(successCampaigns);
        }


    }
}
