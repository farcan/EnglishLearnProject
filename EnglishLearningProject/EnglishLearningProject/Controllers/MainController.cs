using EnglishLearningProject.Extensions;
using EnglishLearningProject.Models;
using EnglishLearningProject.Services;
using EnglishLearningProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishLearningProject.Controllers
{
    public class MainController : Controller
    {


        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly EmailService _emailservice;

        public MainController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager, EmailService emailService) { 
            
            userManager= _userManager;
            signInManager= _signInManager;
            _emailservice= emailService;    
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterViewModel request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var user = new AppUser
                {
                    Name = request.name,
                    Surname = request.surname,
                    Email = request.email,
                    UserName = request.userName
                };
                var identityResult = await userManager.CreateAsync(user, request.password!);

                if (identityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "true";
                    return RedirectToAction("Login","Main");
                }

                foreach (IdentityError item in identityResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
                    
                return View();
            }
            catch (Exception)
            {

                throw;  
            }
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel request)
        {

            try
            {
                var identityResult = await signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

                if (identityResult.Succeeded)
                {
                    TempData["SuccessMessage"] = "true";

                    return RedirectToAction("Index","Member");
                }

                if (identityResult.Succeeded==false)
                {
                    ModelState.AddModelErrorList(new List<string> { "Hatalı Giriş" });
                }

                if (identityResult.IsLockedOut)
                {
                    ModelState.AddModelErrorList(new List<string> { "Kullanıcı Kilitlenmiştir 1 dakika sonra tekrar deneyiniz" });
                }

            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }


        public IActionResult ResetPassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {


            var hasUser = await userManager.FindByEmailAsync(request.Email);
            if (hasUser == null)
            {
                ModelState.AddModelErrorList(new List<string> { "Email Kayıtlı değildir." });
                return View();
            }

            string passwordResetsToken = await userManager.GeneratePasswordResetTokenAsync(hasUser);

            var passwordResetLink = Url.Action("ChangeResetPassword", "Main", new { userId = hasUser.Id, Token = passwordResetsToken }, HttpContext.Request.Scheme);

            //EmailService Lazım.

            await _emailservice.SendResetPasswordEmail(passwordResetLink, request.Email);

            TempData["success"] = "Şifre Yenileme Linki Eposta Adresinize Gönderilmiştir.";

            return RedirectToAction(nameof(ResetPassword));
        }


        public IActionResult ChangeResetPassword(string userId, string token)
        {
            TempData["userId"] = userId;
            TempData["token"] = token;

            if (userId == null || token == null)
            {
                throw new Exception("Bir Hata Meydana Geldi");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeResetPassword(ChangeResetPasswordViewModel request)
        {
            var userId = TempData["userId"].ToString();
            var token = TempData["token"].ToString();
            var hasUser = await userManager.FindByIdAsync(userId!);

            if (hasUser == null)
            {
                ModelState.AddModelErrorList(new List<string> { "Kullanıcı Bulunamamıştır. " });
                return View();
            }

            IdentityResult result = await userManager.ResetPasswordAsync(hasUser, token!, request.Password);

            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Şifreniz başarıyla yenilenmiştir.";
            }
            else
            {
                ModelState.AddModelErrorList(result.Errors.Select(x => x.Description).ToList());

            }

            return RedirectToAction("Login", "Main");
        }



    }
}
