using EnglishLearningProject.Extensions;
using EnglishLearningProject.Models;
using EnglishLearningProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnglishLearningProject.Controllers
{
    public class MainController : Controller
    {


        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public MainController(UserManager<AppUser> _userManager,SignInManager<AppUser> _signInManager) { 
            
            userManager= _userManager;
            signInManager= _signInManager;
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
        
    }
}
