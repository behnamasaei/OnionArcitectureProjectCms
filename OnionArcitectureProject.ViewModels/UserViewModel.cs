using Microsoft.AspNetCore.Identity;
using OnionArcitectureProject.Entities;
using System.ComponentModel.DataAnnotations;

namespace OnionArcitectureProject.ViewModels
{

    public class UserViewModel : ApplicationUser
    {

        public string? Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }


        public string? ProfilePicture { get; set; }


        public string? Role { get; set; }


        public IList<string>? Roles { get; set; }
    }
}
