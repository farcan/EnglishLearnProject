using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class ChangeResetPasswordViewModel
    {
        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre : ")]
        public string Password { get; set; }



        [Compare(nameof(Password), ErrorMessage = "Şifreler Aynı Değildir.")]
        [Required(ErrorMessage = "Şifre Tekrar Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre Tekrar : ")]
        public string ConfirmPassword { get; set; }
    }
}
