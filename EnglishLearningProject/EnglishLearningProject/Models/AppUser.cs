using Microsoft.AspNetCore.Identity;

namespace EnglishLearningProject.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Age { get; set; }
        public int quizQuestionCount { get; set; }
        public ICollection<Word>? Words { get; set; }

        public ICollection<Quiz>? quizzes { get; set; }
    }
}
