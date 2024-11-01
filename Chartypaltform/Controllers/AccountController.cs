using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.Service;
using Chartypaltform.ViewModels;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Chartypaltform.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, 
            RoleManager<IdentityRole> roleManager, IWebHostEnvironment hostEnvironment, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = hostEnvironment; 
            _context = context; 
        }

        [AllowAnonymous]

        [HttpGet]
        public IActionResult RegisterOrganization()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]

        public async Task<IActionResult> RegisterOrganization(CharityOrganizationRegistrationModel model)
        {
            if (ModelState.IsValid)
            {


                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View(model);
                }
                if (model.formFile != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var fileUrl = await FileUploadHelper.HandleFileUpload(model.formFile, uploadsFolder);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        model.Img = fileUrl; 
                    }
                    else
                    {
                        ModelState.AddModelError("", "File upload failed.");
                        
                        return View(model);
                    }
                }
               

                var organization = new CharityOrganization
                {
                    UserName = model.Email,
                    Email = model.Email,
                    OrganizationName = model.OrganizationName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    RegisteredAt = DateTime.Now,
                    FacebookLink = model.FacebookLink,
                    GoogleLink = model.GoogleLink,
                    registration_status = RegistrationStatus.Pending,
                    Img = model.Img,
                };
                await EnsureRoleExists("Organization");


                var result = await _userManager.CreateAsync(organization, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(organization, "Organization");

                    await _signInManager.SignInAsync(organization, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult DonorRegister()
        {
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> DonorRegister(DonorRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is already taken
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View(model);
                }
                if (model.formFile != null)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    var fileUrl = await FileUploadHelper.HandleFileUpload(model.formFile, uploadsFolder);

                    if (!string.IsNullOrEmpty(fileUrl))
                    {
                        model.Img = fileUrl;
                    }
                    else
                    {
                        ModelState.AddModelError("", "File upload failed.");

                        return View(model);
                    }
                }

                var donor = new Donor
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    RegisteredAt = DateTime.Now,
                    Img = model.Img,
                    Age = model.Age,
                    Gender = model.Gender,  
                };
                await EnsureRoleExists("Donor");


                var result = await _userManager.CreateAsync(donor, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(donor, "Donor");
                    await _signInManager.SignInAsync(donor, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
      
      
       
        [HttpGet]
        [AllowAnonymous]

        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]


        public async Task<IActionResult> CreateAdmin(AdminRegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError(string.Empty, "Email is already registered.");
                    return View(model);
                }

                var adminUser = new AdminUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AdminFullName = model.AdminFullName,
                    Address = model.Address,
                    PhoneNumber = model.PhoneNumber,
                    RegisteredAt = DateTime.UtcNow , 
                    Img = "k"
                };
                await EnsureRoleExists("Admin");
                var result = await _userManager.CreateAsync(adminUser, model.Password);
                if (result.Succeeded)
                {
                    TempData["success"] = "goooooooooooooooooooood";
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                    await _signInManager.SignInAsync(adminUser, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }


      
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                        return View(model);
                    }

                    if (await _userManager.IsInRoleAsync(user, "Donor"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Organization"))
                    {
                        var organizationUser = user as CharityOrganization;
                        if (organizationUser != null)
                        {
                            if (organizationUser.registration_status == RegistrationStatus.Approved)
                            {
                                return RedirectToAction("Index", "Home");
                            }
                            else if (organizationUser.registration_status == RegistrationStatus.Pending)
                            {
                                ModelState.AddModelError(string.Empty, "Your organization status is pending. Please wait for approval.");
                            }
                            else if (organizationUser.registration_status == RegistrationStatus.Rejected)
                            {
                                ModelState.AddModelError(string.Empty, "Your organization has been rejected. Contact support for more information.");
                            }
                        }
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        return RedirectToAction("Index", "Cpanel", new { area = "Administrator" });
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null && user.PhoneNumber == model.PhoneNumber)
                {
                    return RedirectToAction("ResetPassword", new { email = model.Email });
                }

                ModelState.AddModelError(string.Empty, "Invalid email or phone number.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email 
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid email.");
                }
            }
            return View(model);
        }


     

    }
}
