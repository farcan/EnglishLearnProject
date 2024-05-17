namespace EnglishLearningProject.ViewModels
{
    public class WordSuccessTableViewModel
    {
        public WordSuccessTableViewModel() { }


        public string WordEN { get; set; }
        public string WordTR { get; set; }
        public int TrueCount { get; set; }
        public int FalseCount { get; set; }
        public double succesRate { get; set; }
    }
}
