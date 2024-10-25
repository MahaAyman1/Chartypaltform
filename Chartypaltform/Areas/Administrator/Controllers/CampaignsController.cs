 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Chartypaltform.Data;
using Chartypaltform.Models;
using System.Security.Claims;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    //act as No catgory 
    public class CampaignsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CampaignsController(ApplicationDbContext context)
        {
            _context = context;
        }

       
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Campaigns
				.Include(c => c.User).Where(c => c.Status != CampaignStatus.Closed)
                .OrderBy(c => c.Status == CampaignStatus.Pending ? 0 : 1) // 'Pending' campaigns come first
				.ThenByDescending(c => c.CreatedAt); 

			return View(await applicationDbContext.ToListAsync());
		}




		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var campaign = await _context.Campaigns
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CampaignId == id);
            if (campaign == null)
            {
                return NotFound();
            }

            return View(campaign);
        }

        
        private bool CampaignExists(int id)
        {
            return _context.Campaigns.Any(e => e.CampaignId == id);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int campaignId, CampaignStatus status, string reason)
        {
            var campaign = await _context.Campaigns
                                         .FirstOrDefaultAsync(c => c.CampaignId == campaignId);
            if (campaign == null)
            {
                return NotFound();
            }

            var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = await _context.Users
                                          .OfType<AdminUser>()
                                          .FirstOrDefaultAsync(u => u.Id == adminId);

            if (adminUser == null)
            {
                return NotFound("Admin user not found.");
            }

            var adminAction = new AdminAction
            {
                ActionType = status == CampaignStatus.Closed ? ActionType.RejectCampaign : ActionType.ApproveCampaign,
                ActionDetails = $"Campaign '{campaign.CampaignName}' status updated to '{status}'.",
                Reason = reason,
                Timestamp = DateTime.UtcNow,
                AdminId = adminId,
                AdminUser = adminUser
            };

            _context.AdminActions.Add(adminAction);

            campaign.Status = status;
            _context.Campaigns.Update(campaign);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}