using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Email alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Email formatı yanlıştır.")]
        [Display(Name = "Email : ")]

        public string Email { get; set; }

    }
}
