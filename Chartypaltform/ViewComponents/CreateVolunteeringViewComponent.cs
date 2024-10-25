using Chartypaltform.Data;
using Chartypaltform.Models;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Chartypaltform.ViewComponents
{
    public class CreateVolunteeringViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public CreateVolunteeringViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        // Render form (GET request)
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new CreateVolunteeringViewModel();
            return View(model); // Load the form view
        }


    }
}
