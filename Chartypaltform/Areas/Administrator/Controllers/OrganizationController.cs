using Chartypaltform.Data;
using Chartypaltform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            // Fetch all organizations with Pending status
            var pendingOrganizations = await _context.CharityOrganizations
                .Where(org => org.registration_status == RegistrationStatus.Pending)
                .ToListAsync();

            return View(pendingOrganizations);
        }

        [HttpPost]
        public async Task<IActionResult> ApproveOrganization(string id) // Change int to string
        {
            var organization = await _context.CharityOrganizations.FindAsync(id); // The id should be a string
            if (organization != null)
            {
                organization.registration_status = RegistrationStatus.Approved;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPendingOrganizations");
        }

        [HttpPost]
        public async Task<IActionResult> RejectOrganization(string id) // Change int to string
        {
            var organization = await _context.CharityOrganizations.FindAsync(id); // The id should be a string
            if (organization != null)
            {
                organization.registration_status = RegistrationStatus.Rejected;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("ShowPendingOrganizations");
        }
    }
}
