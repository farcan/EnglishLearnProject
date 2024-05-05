using EnglishLearningProject.Models;
using EnglishLearningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EnglishLearningProject.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private AppDbContext appDbContext;
        private readonly UserManager<AppUser> userManager;

        public MemberController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
        }

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
        public async Task<IActionResult> AddWord(AddWordViewModel request)
        {

            //try catch mekanizmaları eklenecek ve hata yakalamalar eklenecek.
            AppUser user = await  userManager.FindByNameAsync(User.Identity.Name);
            Word word = new Word { wordEN = request.WordEN, wordTR = request.WordTR, wordSentences = request.WordSentences, UserID = user.Id };
            appDbContext.Word.Add(word);
            appDbContext.SaveChanges();
            return View();
        }

        public async Task<IActionResult> GetWords()
        {
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            var words = await appDbContext.Word.ToListAsync();
            var filteredWords = words.Where(x => x.UserID == user.Id).ToList();
            return View(filteredWords);
        }



        public async Task<IActionResult> DeleteWord(int WordID)
        {
            var word =  await appDbContext.Word.FirstOrDefaultAsync(x=>x.WordID == WordID);
            appDbContext.Word.Remove(word);
            return RedirectToAction("GetWords");
        }


        [HttpGet]
        public async IActionResult UpdateWord(int WordID) {

            


            return View(); 
        }

      

       
    }
}
