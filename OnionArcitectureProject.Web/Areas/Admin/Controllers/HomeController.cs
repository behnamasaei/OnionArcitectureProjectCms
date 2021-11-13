using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnionArcitectureProject.Web.Areas.Admin.Controllers
{
    
    [Authorize(Roles = "SuperAdmin,Admin,Writer")]
    [Area("Admin")]
    public class HomeController : Controller 
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
