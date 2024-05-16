using Microsoft.AspNetCore.Cors;
using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class UserLoginViewModel
    {

        public UserLoginViewModel()
        {
        }

        [Required(ErrorMessage = "Kullanıcı Adı Boş Bırakılamaz")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required(ErrorMessage ="Şifre Alanı Boş Bırakılamaz")]
        [Display(Name ="Şifre")]
        public string Password { get; set; }
    }
    
}
