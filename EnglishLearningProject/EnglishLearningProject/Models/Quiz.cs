using Newtonsoft.Json;

namespace EnglishLearningProject.Models
{
    public class Quiz
    {
        public int? quizID { get; set; }
        public int counter { get; set; } = 0;
        public int level { get; set; } = 0;
        public DateTime? replyDate { get; set; } = null;

        public string? UserID { get; set; }
        public int? WordID { get; set; }
        
        public AppUser? AppUser { get; set; }
        public Word? Word { get; set; }

        [JsonIgnore]
        public ICollection<TestLog> testLogs { get; set; }
    }

}
