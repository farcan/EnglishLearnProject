using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class AddWordViewModel
    {

        [Required(ErrorMessage = "Lütfen İngilizce Kelime Giriniz")]
        [Display(Name = "İngilizce Kelime")]
        public string WordEN { get; set; }

        [Required(ErrorMessage = "Lütfen Türkçe Kelime Giriniz")]
        [Display(Name = "Türkçe Kelime")]
        public string WordTR { get; set; }

        [Required(ErrorMessage = "Lütfen İngilizce Cümle Giriniz")]
        [Display(Name = "İngilizce Cümle")]
        public string WordSentences { get; set; }
        public string UserID { get; set; }
    }
}
