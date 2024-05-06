using EnglishLearningProject.Models;
using EnglishLearningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
            var test = CreateRandomWord(3);
            return View(filteredWords);
        }



        public async Task<IActionResult> DeleteWord(int WordID)
        {
            var word =  await appDbContext.Word.FirstOrDefaultAsync(x=>x.WordID == WordID);
            appDbContext.Word.Remove(word);
            return RedirectToAction("GetWords");
        }


        [HttpGet]
        public async Task<IActionResult> UpdateWord(int WordID) {

            var word = await appDbContext.Word.FirstOrDefaultAsync(x => x.WordID == WordID);
            var model = new UpdateWordViewModel { WordID = WordID, UserID = word.UserID, WordEN= word.wordEN,WordTR= word.wordTR,WordSentences = word.wordSentences };

            return View(model); 
        }

        public async Task<IActionResult> UpdateWord(UpdateWordViewModel req)
        {
            var updatedWord = await appDbContext.Word.FirstOrDefaultAsync(x => x.WordID == req.WordID);
            updatedWord.wordEN = req.WordEN;
            updatedWord.wordTR = req.WordTR;
            updatedWord.wordSentences = req.WordSentences;

            appDbContext.Word.Update(updatedWord);
            appDbContext.SaveChanges();
            
            return RedirectToAction("GetWords");
        }

         
        public async Task<List<Word>> CreateRandomWord(int wordCount)
        {
            List<Word> words = new List<Word>();

            var user = userManager.FindByNameAsync(User.Identity.Name);
            var dbContextWords =  appDbContext.Word.Where(x => x.UserID == user.Result.Id).ToList(); // UserID kısıtlaması yapılmış veriler.
            int dbContextWordsRange = dbContextWords.Count();          
            /*
             Random sayı ürettir 0 ile dbContextWordsRange arasında
            Daha sonra o sayıyı sec ve dbdeki veriden çek.
            words listesinde olup olmamasını kontrol et.
             */
            for (int i = 0; i < wordCount ; i++)
            {
      
                Random rnd = new Random();
                int randomIndex = rnd.Next(0, dbContextWordsRange);
                var selectedWord = dbContextWords[randomIndex];
                
                //recursive structure
                if (words.Contains(selectedWord))
                {
                    List<Word> newWordList = CreateRandomWord(1).Result;
                    words.Add(newWordList[0]);
                    
                }
                else
                {
                 words.Add(selectedWord);
                }
            }

            return words;
        }
        

        public void CreateQuiz()
        {

         
           
        }









       
    }
}
