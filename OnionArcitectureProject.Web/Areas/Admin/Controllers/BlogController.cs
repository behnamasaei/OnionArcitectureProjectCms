using Microsoft.AspNetCore.Mvc;
using OnionArcitectureProject.Entities;

namespace OnionArcitectureProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult NewPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewPost(BlogPost model)
        {
            return View();
        }
    }
}
