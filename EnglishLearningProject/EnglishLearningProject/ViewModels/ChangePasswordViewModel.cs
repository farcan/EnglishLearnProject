using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Eski Şifre Alanı Boş Bırakılamaz.")]
        [Display(Name = "Eski Şifre")]
        public string oldPassword { get; set; } = null!;




        [Required(ErrorMessage = "Yeni Şifre Alanı Boş Bırakılamaz")]
        [Compare(nameof(newConfirmedPassword), ErrorMessage = "Yeni Şifreler Aynı Değildir.")]
        [Display(Name = "Yeni Şifre")]
        public string newPassword { get; set; } = null!;



        [Required(ErrorMessage = "Yeni Şifre Kontrol Alanı Boş Bırakılamaz.")]
        [Display(Name = "Yeni Şifre Kontrol")]
        public string newConfirmedPassword { get; set; } = null!;
    }
}
