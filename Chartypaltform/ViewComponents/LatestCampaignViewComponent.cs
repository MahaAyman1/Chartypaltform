﻿using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.ViewComponents
{
    public class LatestCampaignViewComponent : ViewComponent
    {

        private readonly ApplicationDbContext _db;

        public LatestCampaignViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var campaigns = await _db.Campaigns
                  .Include(c => c.User).Where(c => c.Status == CampaignStatus.Active)
                  .OrderByDescending(c => c.CreatedAt) 
                  .Take(5) 
                  .ToListAsync();


            return View(campaigns);
        }

    }
    
}
