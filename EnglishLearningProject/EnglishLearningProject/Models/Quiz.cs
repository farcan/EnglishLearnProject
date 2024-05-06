namespace EnglishLearningProject.Models
{
    public class Quiz
    {
        public int quizID { get; set; }
        public int counter { get; set; } = 0;
        public int level { get; set; } = 0;
        public DateTime? replyDate { get; set; }

        public string? UserID { get; set; }
        public int WordID { get; set; }
        
        public AppUser? AppUser { get; set; }
        public Word? Word { get; set; }
    }

}
