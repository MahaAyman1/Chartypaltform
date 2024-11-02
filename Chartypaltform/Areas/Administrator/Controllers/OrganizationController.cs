
using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize]
    public class OrganizationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrganizationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowPendingOrganizations()
        {
            var pendingOrganizations = await _context.CharityOrganizations
                .Where(org => org.registration_status == RegistrationStatus.Pending)
                .ToListAsync();

            return View(pendingOrganizations);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrganization(string id, string reason)
        {
            var organization = await _context.CharityOrganizations.FindAsync(id);
            if (organization != null)
            {
                organization.registration_status = RegistrationStatus.Approved;
                await _context.SaveChangesAsync();

                // Log admin action
                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var adminUser = await _context.Users.OfType<AdminUser>()
                                                     .FirstOrDefaultAsync(u => u.Id == adminId);

                if (adminUser != null)
                {
                    var adminAction = new AdminAction
                    {
                        ActionType = ActionType.AcceptOrganization,
                        ActionDetails = $"Organization '{organization.OrganizationName}' status updated to 'Accept'.",
                        Reason = reason, 
                        Timestamp = DateTime.UtcNow,
                        AdminId = adminId,
                        AdminUser = adminUser
                    };
                    _context.AdminActions.Add(adminAction); 
                    await _context.SaveChangesAsync();
                }

                TempData["success"] = "Organization Accept successfully.";
            }
            else
            {
                TempData["error"] = "Organization not found.";
            }

            return RedirectToAction("ShowPendingOrganizations");
        }

        [HttpPost]
        public async Task<IActionResult> RejectOrganization(string id, string reason)
        {
            var organization = await _context.CharityOrganizations.FindAsync(id);
            if (organization != null)
            {
                organization.registration_status = RegistrationStatus.Rejected;
                await _context.SaveChangesAsync();

                // Log admin action
                var adminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var adminUser = await _context.Users.OfType<AdminUser>()
                                                     .FirstOrDefaultAsync(u => u.Id == adminId);

                if (adminUser != null)
                {
                    var adminAction = new AdminAction
                    {
                        ActionType = ActionType.RejectOrganization,
                        ActionDetails = $"Organization '{organization.OrganizationName}' status updated to 'Rejected'.",
                        Reason = reason, 
                        Timestamp = DateTime.UtcNow,
                        AdminId = adminId,
                        AdminUser = adminUser
                    };
                    _context.AdminActions.Add(adminAction); 
                    await _context.SaveChangesAsync();
                }

                TempData["success"] = "Organization rejected successfully.";
            }
            else
            {
                TempData["error"] = "Organization not found.";
            }
            return RedirectToAction("ShowPendingOrganizations");
        }

        public async Task<IActionResult> Details(string id)
        {
            var organization = await _context.Users.OfType<CharityOrganization>()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (organization == null)
            {
                return NotFound();
            }

            return View(organization);
        }
    }
}
