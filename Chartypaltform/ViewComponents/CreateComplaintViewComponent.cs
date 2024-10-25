using Chartypaltform.Data;
using Chartypaltform.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chartypaltform.ViewComponents
{
    public class CreateComplaintViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public CreateComplaintViewComponent(ApplicationDbContext db)
        {
            _db = db;   
        }
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(); 
        }

    }
}
