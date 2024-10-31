using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize]
    public class CpanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
		private readonly ApplicationDbContext _context;
        public CpanelController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
			RoleManager<IdentityRole> roleManager , ApplicationDbContext context)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
			_context = context;	
        }


		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);

			if (user is AdminUser adminUser)
			{
				var openCampaignsCount = await _context.Campaigns
					.CountAsync(c => c.Status != CampaignStatus.Closed);

				var approvedOrganizationsCount = await _context.CharityOrganizations
					.CountAsync(org => org.registration_status == RegistrationStatus.Approved);

				var registeredUserCount = await _userManager.Users.CountAsync();

				// Calculate weekly growth rate
				var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
				var lastWeekUserCount = await _userManager.Users
					.CountAsync(u => u.RegisteredAt <= oneWeekAgo);
				var weeklyGrowthRate = lastWeekUserCount > 0
					? (double)(registeredUserCount - lastWeekUserCount) / lastWeekUserCount * 100
					: 0;

				var model = new AdminDashboardViewModel
				{
					AdminFullName = adminUser.AdminFullName,
					RegisteredAt = adminUser.RegisteredAt,
					PhoneNumber = adminUser.PhoneNumber,
					Address = adminUser.Address,
					OpenCampaignsCount = openCampaignsCount,
					ApprovedOrganizationsCount = approvedOrganizationsCount,
					RegisteredUserCount = registeredUserCount,
					WeeklyUserGrowthRate = weeklyGrowthRate,
				};

				return View(model);
			}
			return View();
		}


		public IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
		

	}
}