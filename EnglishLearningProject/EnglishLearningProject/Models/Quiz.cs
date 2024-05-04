namespace EnglishLearningProject.Models
{
    public class Quiz
    {
        public int quizID { get; set; }
        public int counter { get; set; }
        public int level { get; set; }
        public DateTime? replyDate { get; set; }

        public string? UserID { get; set; }
        public int WordID { get; set; }
        
        public AppUser? AppUser { get; set; }
        public Word? Word { get; set; }
    }

}
