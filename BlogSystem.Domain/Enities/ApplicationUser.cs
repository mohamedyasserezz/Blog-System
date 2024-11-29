using Microsoft.AspNetCore.Identity;

#nullable disable

namespace BlogSystem.Domain.Enities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; } 
    }
}
