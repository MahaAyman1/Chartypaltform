using Chartypaltform.Data;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Chartypaltform.Areas.Administrator.Controllers
{    [Area("Administrator")]

    public class AdminActionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminActionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Fetch all AdminActions with the corresponding AdminUser based on AdminId
            var adminActions = await _context.AdminActions
                .Include(a => a.AdminUser)  // Include the AdminUser entity using the AdminId
                .ToListAsync();

            // Log or inspect the fetched data
            foreach (var action in adminActions)
            {
                // Check if AdminUser is null
                if (action.AdminUser == null)
                {
                    // Log or breakpoint here to see the action
                    Console.WriteLine($"Action ID {action.Id} has no associated AdminUser.");
                }
            }

            var adminActionViewModels = adminActions.Select(a => new AdminActionViewModel
            {
                Id = a.Id,
                ActionType = a.ActionType,
                ActionDetails = a.ActionDetails,
                Reason = a.Reason,
                Timestamp = a.Timestamp,
                AdminFullName = a.AdminUser != null && !string.IsNullOrEmpty(a.AdminUser.AdminFullName)
                    ? a.AdminUser.AdminFullName
                    : "Unknown Admin" // Fallback to "Unknown Admin"
            }).ToList();

            return View(adminActionViewModels); // Pass the list to the view
        }


    }
}
