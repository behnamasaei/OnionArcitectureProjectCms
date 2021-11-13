using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnionArcitectureProject.Entities;
using OnionArcitectureProject.ViewModels;

namespace OnionArcitectureProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin , SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ListUserAsync()
        {
            var users = _userManager.Users.ToList();
            List<UserViewModel> usersInfo = new List<UserViewModel>();
            foreach (var user in users)
            {
                usersInfo.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return View(usersInfo);
        }
    }
}
