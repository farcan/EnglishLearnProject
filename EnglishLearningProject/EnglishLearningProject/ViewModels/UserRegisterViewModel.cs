using Microsoft.AspNetCore.Cors;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace EnglishLearningProject.ViewModels
{
    public class UserRegisterViewModel
    {
        public UserRegisterViewModel() { }
        public UserRegisterViewModel(string? name, string? surname, string? userName, string? email, string? password)
        {
            this.name = name;
            this.surname = surname;
            this.userName = userName;
            this.email = email;
            this.password = password;
        }


        [Required(ErrorMessage = "İsim Alanı Boş Bırakılamaz")]
        [Display(Name = "İsim")]
        public string? name { get; set; }

        [Required(ErrorMessage = "Soyisim Alanı Boş Bırakılamaz")]
        [Display(Name = "Soyisim")]
        public string? surname { get; set; }

        [Required(ErrorMessage = "Kullanıcı Adı Alanı Boş Bırakılamaz")]
        [Display(Name = "Kullanıcı Adı")]
        public string? userName { get; set; }

        [Required(ErrorMessage = "Enail Alanı Boş Bırakılamaz")]
        [Display(Name = "Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Şifre Alanı Boş Bırakılamaz")]
        [Display(Name = "Şifre")]
        public string? password { get; set; }
       
    }
}
