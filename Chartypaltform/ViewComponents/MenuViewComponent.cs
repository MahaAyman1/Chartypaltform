using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Chartypaltform.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chartypaltform.Models;

namespace Chartypaltform.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MenuViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var navItems = await _context.navItems.ToListAsync();
            var user = await _userManager.GetUserAsync(HttpContext.User);

            bool isOrganizationApproved = false;
            bool isUserDonor = false;

            if (user is CharityOrganization organizationUser)
            {
                isOrganizationApproved = organizationUser.registration_status == RegistrationStatus.Approved;
            }
            else if (user != null)
            {
                isUserDonor = await _userManager.IsInRoleAsync(user, "Donor");
            }

            var visibleNavItems = navItems.Where(item =>
                item.Visibility == "Both" ||
                (item.Visibility == "Authenticated" && user != null && (isOrganizationApproved || isUserDonor)) ||
                (item.Visibility == "Anonymous" && user == null)).ToList();

            return View(visibleNavItems);
        }



    }
}
