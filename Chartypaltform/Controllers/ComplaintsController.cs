using Chartypaltform.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Chartypaltform.Data;
using Chartypaltform.Models;

namespace Chartypaltform.Controllers
{
    [Authorize]
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Complaints/Create
        [Authorize(Roles = "Donor")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Complaints/Create
        [Authorize(Roles = "Donor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ComplaintViewModel complaintVM)
        {
            // Get the UserId from the currently logged-in user
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized(); // Ensure the user is logged in
            }

            if (ModelState.IsValid)
            {
                // Map the view model to the Complaint entity
                var complaint = new Complaint
                {
                    ComplaintText = complaintVM.ComplaintText,
                    CreatedAt = DateTime.Now,
                    Status = ComplaintStatus.Pending,
                    UserId = userId ,
                    Subject = complaintVM.Subject
                };

                // Add the complaint entity to the database context
                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync(); // Save changes to the database

                return RedirectToAction(nameof(Index));
            }

            return View(complaintVM); // If ModelState is not valid, return the view with the view model
        }

        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints.Include(c => c.User).ToListAsync();
            return View(complaints);
        }
    }
}
