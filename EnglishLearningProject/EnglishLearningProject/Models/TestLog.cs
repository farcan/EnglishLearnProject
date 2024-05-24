namespace EnglishLearningProject.Models
{
    public class TestLog
    {
        //Bir Soru çözüldüğünde bu sınıf istatik tutmak için gereklidir.
        //Bir testlog sınıfında bir quiz ismi bulunabilir.
        //Bir quiz sınıfı birden fazla testlog sınıfında bulunabilir.

        public int TestLogID { get; set; }  
        public int? QuizID { get; set; }
        public string trueWord { get; set; }
        public string trueSentences { get; set; }

        public string WordImage { get; set; }
        public string TrueWordTR { get; set; }
        public string falseWord1 { get; set; }     
        public string falseWord2 { get; set; }
        public string falseWord3 { get; set; }
        public string SelectedAnswer { get; set; }
        public DateTime logDate {  get; set; }
        public bool testResult { get; set; }
        public Quiz? Quiz { get; set; }
        
    }
}
