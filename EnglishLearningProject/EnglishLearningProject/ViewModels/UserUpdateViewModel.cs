using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class UserUpdateViewModel
    {

        public UserUpdateViewModel() { }


        [Required(ErrorMessage = " İsim Boş Bırakılamaz")]
        [Display(Name = "İsim")]
        public string Name { get; set; }

        [Required(ErrorMessage = " Soyisim Boş Bırakılamaz")]
        [Display(Name = "Soyisim")]
        public string Surname { get; set; }

        [Required(ErrorMessage = " Email Boş Bırakılamaz")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Boş Bırakılamaz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Quiz Sayısı Boş Bırakılamaz")]
        [Display(Name = "Quiz Sayısı")]
        public int quizQuestionCount { get; set; }
    }
}
