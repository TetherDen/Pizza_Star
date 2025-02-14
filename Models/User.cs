using Microsoft.AspNetCore.Identity;

namespace Pizza_Star.Models
{
    public class User : IdentityUser
    {
        public int Year { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
