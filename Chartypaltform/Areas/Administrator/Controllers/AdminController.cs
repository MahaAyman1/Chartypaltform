using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Chartypaltform.Areas.Administrator.Controllers
{
    [Area("Administrator")]
    [Authorize(Roles = "Admin")] // Restrict access to only Super Admin
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdmin(AdminRegistrationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if an account with this email already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(model);
            }

            // Create new admin user
            var newAdminUser = new AdminUser
            {
                UserName = model.Email,
                Email = model.Email,
                AdminFullName = model.AdminFullName,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                RegisteredAt = DateTime.UtcNow,
                Img = "default_image.png" // Use a clear placeholder
            };

            // Ensure the "Admin" role exists
            await EnsureRoleExists("Admin");

            // Create the new admin user with specified password
            var result = await _userManager.CreateAsync(newAdminUser, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            // Assign the new user to the "Admin" role and sign them in
            await _userManager.AddToRoleAsync(newAdminUser, "Admin");

  
            var superAdminId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var superAdmin = await _context.Users
                                          .OfType<AdminUser>()
                                          .FirstOrDefaultAsync(u => u.Id == superAdminId);
            if (superAdmin == null)
            {
                ModelState.AddModelError(string.Empty, "Super Admin not found.");
                return View(model);
            }

            // Record the admin creation action in the database
            var adminAction = new AdminAction
            {
                ActionType = ActionType.CreateAdmin,
                ActionDetails = $"Admin '{newAdminUser.AdminFullName}' created by '{superAdmin}'.",
                Timestamp = DateTime.UtcNow,
                AdminId = superAdminId,
                AdminUser = superAdmin,
            };
            _context.AdminActions.Add(adminAction);
            await _context.SaveChangesAsync();

            TempData["success"] = "Admin created successfully.";
            return RedirectToAction("Index", "Cpanel");
        }

        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
