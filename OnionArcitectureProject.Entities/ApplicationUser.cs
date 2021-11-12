using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnionArcitectureProject.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? ProfilePictureUser { get; set; }
    }
}
