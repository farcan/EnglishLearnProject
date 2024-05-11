using EnglishLearningProject.Models;
using EnglishLearningProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text.Json.Serialization;


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
            await CreateQuiz();
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
            var dbContextWords =  appDbContext.Word.Where(x => x.UserID == user.Result.Id && x.isLearned==false).ToList(); // UserID kısıtlaması yapılmış veriler.
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
        

        public async Task CreateQuiz()
        {
            //Kaç Kelime Eklenecek ve Kelimelerin Seviyelerine göre kelime ekleme işlemi yazılacak.
            // 0 level kelime sayısı eğer kullanıcının çözmek istediği kelime sayısından az ise yeni kelimeler eklenecek.

            //TestLog ile Quiz arasındaki ilişki 1-1 olabilir. ona ilerleyen süreçlerde karar ver.

            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            var count = appDbContext.Word.Where(x=>x.UserID==user.Id && x.isLearned==false).ToList().Count();
            List<Word> quizWords = new List<Word>();
            quizWords = await CreateRandomWord(3);

            foreach (var item in quizWords)
            {
                var quiz = new Quiz
                {
                    UserID = user.Id,
                    WordID = item.WordID
                };
                appDbContext.Quiz.Add(quiz);
            }
            appDbContext.SaveChanges();

        }

        //Leveli sıfır olmayan quizleride çekecek.
        public async Task<List<Word>> randomFalseAnswer(Word trueWord, List<Quiz> quizList)
        {

            quizList = quizList.ToList();
            List<Word> wordList = new List<Word>();
            foreach (var item in quizList)
            {
                var tmpword = appDbContext.Word.FirstOrDefault(x=>x.WordID == item.WordID); 
                wordList.Add(tmpword);
            }

            Random rnd = new Random();
            List<Word> falseWords = new List<Word>();
            wordList.Remove(trueWord);

            for (int i = 0; i < 3; i++)
            {
                int randomIndex = rnd.Next(0, wordList.Count);
                falseWords.Add(wordList[randomIndex]);
                wordList.Remove(wordList[randomIndex]);
            }

            return falseWords;

        }

        
        public async Task<IActionResult> SolveQuestion()
        {


            var quizListJSON = TempData["QuizList"] as string;
            var quizList = JsonConvert.DeserializeObject<List<Quiz>>(quizListJSON);


            var TestList = new List<TestLog>();

            foreach (var item in quizList)
            {
                var trueWordEN = appDbContext.Word.FirstOrDefault(x => x.WordID == item.WordID).wordEN;
                var trueWordTR = appDbContext.Word.FirstOrDefault(x => x.WordID == item.WordID).wordTR;
                var trueWordSentences = appDbContext.Word.FirstOrDefault(x => x.WordID == item.WordID).wordEN;

                var wrd = appDbContext.Word.FirstOrDefault(x => x.WordID == item.WordID);
                var falseWords = await randomFalseAnswer(wrd, quizList);
                var test = new TestLog
                {
                    QuizID = item.quizID,
                    trueWord = trueWordEN,
                    TrueWordTR = trueWordTR,
                    trueSentences = trueWordSentences,
                    falseWord1 = falseWords[0].wordTR,
                    falseWord2 = falseWords[1].wordTR,
                    falseWord3 = falseWords[2].wordTR
                };
                TestList.Add(test);
            }         
            return View(TestList);
        }


        [HttpPost]
        public IActionResult SaveTestLog( [FromBody] TestLog data)
         {
          /* TestLog data = JsonConvert.DeserializeObject<TestLog>(request);
            appDbContext.TestLog.Add(data);






         //   appDbContext.SaveChanges();*/
            return Ok();
        }


        public async Task<IActionResult> StartTest()
        {

            //Quiz Var mı yok Mu diye kontrol etmeli ve daha sonra quiz yoksa eğer oluşturmalı
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var quizList = appDbContext.Quiz.Where(w => w.UserID == user.Id).ToList();

            //Eğer Hiç Testi Yoksa
            if (quizList.Count==0)
            {
              await CreateQuiz();
            }
            //Eğer test leveli sıfır olanların sayısı user QuestionCounttan az ise yeni oluşturacak. ve ekleyecek
            if (quizList.Where(x=>x.level==0).ToList().Count<user.quizQuestionCount)
            {

            }

            //Test'e giriş.
            if (quizList.Count > 1)
            {

                string quizListJSON =  JsonConvert.SerializeObject(quizList);
                TempData["QuizList"] = quizListJSON;
                return RedirectToAction("SolveQuestion");
            }

            return View();
            
        }


        

        






       
    }
}
