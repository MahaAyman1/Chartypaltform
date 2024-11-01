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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                TempData["error"] = "Failed to update status. user not found.";
                return RedirectToAction("Contact", "Home");

            }

            if (ModelState.IsValid)
            {
                var complaint = new Complaint
                {
                    ComplaintText = complaintVM.ComplaintText,
                    CreatedAt = DateTime.Now,
                    Status = ComplaintStatus.Pending,
                    UserId = userId ,
                    Subject = complaintVM.Subject
                };

                _context.Complaints.Add(complaint);
                await _context.SaveChangesAsync(); 

                return RedirectToAction("Contact", "Home");
            }

            return View(complaintVM); 
        }

        // GET: Complaints
        public async Task<IActionResult> Index()
        {
            var complaints = await _context.Complaints.Include(c => c.User).ToListAsync();
            return View(complaints);
        }
    }
}
