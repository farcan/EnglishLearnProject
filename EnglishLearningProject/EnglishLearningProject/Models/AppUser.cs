using Microsoft.AspNetCore.Identity;

namespace EnglishLearningProject.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Age { get; set; }
    }
}
