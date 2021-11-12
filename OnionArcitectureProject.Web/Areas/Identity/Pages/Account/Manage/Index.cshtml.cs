// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnionArcitectureProject.DataLayer.Context;
using OnionArcitectureProject.Entities;
using System.IO;
using OnionArcitectureProject.Common;

namespace OnionArcitectureProject.Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _uow;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IWebHostEnvironment webHostEnvironment,
            IUnitOfWork uow)
        
        {
            _userManager = userManager;
            _signInManager = signInManager;
            WebHostEnvironment = webHostEnvironment;
            _uow = uow;
        }

        public string  ImagePath { get; set; }
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }
        public IWebHostEnvironment WebHostEnvironment { get; }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name ="Profile Picture")]
            public string ProfilePictureUser { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var profilePicture = user.ProfilePictureUser;

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                ProfilePictureUser = profilePicture
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            ImagePath = user.ProfilePictureUser;

            await LoadAsync(user);
            return Page();
        }

        

        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                
                await LoadAsync(user);
                return Page();
            }

            if (file != null)
            {
                // delete curent image user then save new image user
                string deleteImagePath = Path.Combine(WebHostEnvironment.WebRootPath,
                        "User\\Images", user.ProfilePictureUser);

                if (System.IO.File.Exists(deleteImagePath))
                    System.IO.File.Delete(deleteImagePath);

                UniqData uniqData = new UniqData();
                string fileName = file.FileName.Trim('"');
                fileName = uniqData.GetUniqueName(fileName);

                _uow.UploadImage(file , fileName);
                user.ProfilePictureUser =  fileName;
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            //if(Input.ProfilePictureUser != null)
            //{
            //    string existfile = Path.Combine( WebHostEnvironment.WebRootPath,
            //        "User/Images", user.ProfilePictureUser);
            //    System.IO.File.Delete(existfile);

            //    var uploads = Path.Combine(WebHostEnvironment.WebRootPath,
            //        "User/Images" , user.ProfilePictureUser);
            //    var filePath = Path.Combine(uploads, fileName);
            //    this.Image.CopyTo(new FileStream(filePath, FileMode.Create));
            //    user.Avatar = fileName; // Set the file name
            //}



            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
