using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnionArcitectureProject.Entities;
using OnionArcitectureProject.ViewModels;

namespace OnionArcitectureProject.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin , SuperAdmin")]
    public class SettingController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public SettingController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
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



        [HttpGet]
        public async Task<IActionResult> RemoveUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            List<string> roleUser = (List<string>)await _userManager.GetRolesAsync(user);
            List<string> roleID = new List<string>();
            foreach (var role in roleUser)
            {
                roleID.Add(_roleManager.Roles.FirstOrDefault(r => r.Name == role).Id.ToString());
            }
            if (user != null)
            {
                // remove automate role`s user
                IdentityResult result = await _userManager.DeleteAsync(user);

                if (result.Succeeded)
                    return RedirectToAction("ListUser");
                else
                    return BadRequest();
            }
            else
                return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userInfo = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            };

            return View(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel user)
        {
            var userInfo = await _userManager.FindByIdAsync(user.Id);
            if (userInfo != null)
            {
                if (!string.IsNullOrEmpty(userInfo.Email))
                    userInfo.Email = user.Email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(userInfo.UserName))
                    userInfo.UserName = user.UserName;
                else
                    ModelState.AddModelError("", "UserName cannot be empty");


                if (!string.IsNullOrEmpty(userInfo.PasswordHash))
                    userInfo.PasswordHash = _passwordHasher.HashPassword(userInfo, user.Password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(userInfo.Email) && !string.IsNullOrEmpty(userInfo.PasswordHash))
                {
                    IdentityResult result = await _userManager.UpdateAsync(userInfo);
                    if (result.Succeeded)
                        return RedirectToAction("ListUser");
                    else
                        return BadRequest();
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> DetailUser(string id)
        {
            if(!string.IsNullOrEmpty(id))
            {
                var user = await _userManager.FindByIdAsync(id);

                var userInfo = new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    ProfilePicture = user.ProfilePictureUser,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    TwoFactorEnabled = user.TwoFactorEnabled,
                    Roles = await _userManager.GetRolesAsync(user)
                };
                return View(userInfo);
            }
            
            return View();
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                var userInfo = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Email = user.Email
                };

                await _userManager.CreateAsync(userInfo, user.Password);
                await _userManager.AddToRoleAsync(userInfo, user.Role); 
                return RedirectToAction("ListUser");
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                ModelState.AddModelError(nameof(allErrors), allErrors.ToString());
            }
            return View(user);
        }
    }
}

