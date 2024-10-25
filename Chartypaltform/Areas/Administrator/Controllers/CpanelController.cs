using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize]
    public class CpanelController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public CpanelController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is AdminUser adminUser)
            {
                var model = new AdminDashboardViewModel
                {
                    AdminFullName = adminUser.AdminFullName,
                    RegisteredAt = adminUser.RegisteredAt,
                    PhoneNumber = adminUser.PhoneNumber,
                    Address = adminUser.Address,
                };

                return View(model);
            }
            return View();
        }
        public IActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home");
        }
		public IActionResult LoginA()
		{
			return View();
		}


	}
}