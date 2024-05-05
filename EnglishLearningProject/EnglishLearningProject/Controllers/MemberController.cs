using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnglishLearningProject.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult AddWord()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddWord()
        {
            return View();
        }
    }
}
