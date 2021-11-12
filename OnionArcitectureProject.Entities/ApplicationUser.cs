using Microsoft.AspNetCore.Identity;

namespace OnionArcitectureProject.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePictureUser { get; set; }
    }
}
