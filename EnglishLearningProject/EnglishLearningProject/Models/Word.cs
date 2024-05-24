namespace EnglishLearningProject.Models
{
    public class Word
    {
        public int? WordID { get; set; }
        public string? wordTR { get; set; }
        public string? wordEN { get; set; }  
        public string? wordSentences { get; set; }
        public string? UserID { get; set; }

        public string? wordImage { get; set; }
        public bool? isLearned { get; set; } = false;

        public AppUser? user { get; set; }

        public ICollection<Quiz>? Quizs { get; set; }
    }
}
