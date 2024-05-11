using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace EnglishLearningProject.Models
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public int Age { get; set; }
        public int quizQuestionCount { get; set; } = 0;
        public ICollection<Word>? Words { get; set; }


        [JsonIgnore]
        public ICollection<Quiz>? quizzes { get; set; }
    }
}
