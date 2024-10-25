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
using System.Drawing;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    public class ComplaintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ComplaintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Complaints.Include(c => c.User).Where(c => c.Status == ComplaintStatus.Pending); 
            return View(await applicationDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var complaint = await _context.Complaints
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.ComplaintId == id);
            if (complaint == null)
            {
                return NotFound();
            }

            return View(complaint);
        }

        private bool ComplaintExists(int id)
        {
            return _context.Complaints.Any(e => e.ComplaintId == id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ComplaintStatus status, string solution)
        {
            var complaint = await _context.Complaints.FindAsync(id);
            if (complaint == null)
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


            if (status == ComplaintStatus.Dismissed)
            {
                _context.Complaints.Remove(complaint);
            }
            else
            {
                complaint.Status = status;
            }
            await _context.SaveChangesAsync();

            ActionType actionType;
            switch (status)
            {
                case ComplaintStatus.Dismissed:
                    actionType = ActionType.DismissedComplaints; 
                    break;
                case ComplaintStatus.Resolved:
                    actionType = ActionType.ResolvedComplaints; 
                    break;
                case ComplaintStatus.Pending:
                default:
                    actionType = ActionType.PendingComplaints; 
                    break;
            }

            var adminAction = new AdminAction
            {
                ActionType = actionType,
                ActionDetails = $"complaint  : '{complaint.ComplaintId}' status updated to '{status}'.",
                Reason = solution,
                Timestamp = DateTime.UtcNow,
                AdminId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                AdminUser = adminUser

            };

            _context.AdminActions.Add(adminAction);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



    }
}
