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
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using DinkToPdf;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Azure.Core;
using EnglishLearningProject.Extensions;



namespace EnglishLearningProject.Controllers
{

    [Authorize]
    public class MemberController : Controller
    {

        private AppDbContext appDbContext;
        private readonly UserManager<AppUser> userManager;
        private readonly ICompositeViewEngine _viewEngine;
        private readonly SignInManager<AppUser> signInManager;


        public MemberController(AppDbContext appDbContext, UserManager<AppUser> userManager, ICompositeViewEngine viewEngine, SignInManager<AppUser> signInManager)
        {
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            _viewEngine = viewEngine;
            this.signInManager = signInManager;
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
            AppUser user = await userManager.FindByNameAsync(User.Identity.Name);
            Word word = new Word { wordEN = request.WordEN, wordTR = request.WordTR, wordSentences = request.WordSentences, UserID = user.Id };
            appDbContext.Word.Add(word);
            appDbContext.SaveChanges();
            return RedirectToAction("GetWords", "Member");
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
            var word = await appDbContext.Word.FirstOrDefaultAsync(x => x.WordID == WordID);
            appDbContext.Word.Remove(word);
            appDbContext.SaveChanges();
            return RedirectToAction("GetWords");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWord(int WordID)
        {

            var word = await appDbContext.Word.FirstOrDefaultAsync(x => x.WordID == WordID);
            var model = new UpdateWordViewModel { WordID = WordID, UserID = word.UserID, WordEN = word.wordEN, WordTR = word.wordTR, WordSentences = word.wordSentences };

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
            var dbContextWords = appDbContext.Word.Where(x => x.UserID == user.Result.Id && x.isLearned == false).ToList(); // UserID kısıtlaması yapılmış veriler.
            int dbContextWordsRange = dbContextWords.Count();
            /*
             Random sayı ürettir 0 ile dbContextWordsRange arasında
            Daha sonra o sayıyı sec ve dbdeki veriden çek.
            words listesinde olup olmamasını kontrol et.
             */
            for (int i = 0; i < wordCount; i++)
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


            //Eğer 0 kelime var ise uyarı vermeli.
            //Eğer Appuser da kelime sayısı 0 ise kendi en az 4 kelime istemeli ve bunları sormalı.



            AppUser? user = await userManager.FindByNameAsync(User.Identity.Name);
            var count = appDbContext.Word.Where(x => x.UserID == user.Id && x.isLearned == false).ToList().Count();
            List<Word> quizWords = new List<Word>();
            if (user.quizQuestionCount == 0)
            {
                Console.WriteLine("quizQuestionWord Tanımlı Değil");
                if (count == 0)
                {
                    Console.Write("Yetersiz Kelime");

                }
                if (count >= 4)
                {
                    quizWords = await CreateRandomWord(4);

                    foreach (var item in quizWords)
                    {
                        var quiz = new Quiz
                        {
                            UserID = user.Id,
                            WordID = item.WordID
                        };
                        appDbContext.Quiz.Add(quiz);
                    }
                }
            }
            if (user.quizQuestionCount < count)
            {
                quizWords = await CreateRandomWord(user.quizQuestionCount);

                foreach (var item in quizWords)
                {
                    var quiz = new Quiz
                    {
                        UserID = user.Id,
                        WordID = item.WordID
                    };
                    appDbContext.Quiz.Add(quiz);
                }
            }
            appDbContext.SaveChanges();
        }

        //tüm kelimeler bittiğinde buradaki missingWords listesi boş kalacağı için hata verecektir.
        public async Task CreateNewQuiz(int count, List<Quiz> quizlist)
        {

            var user = await userManager.FindByNameAsync(User.Identity.Name);

            // var allQuizList = appDbContext.Quiz.Where(x => x.UserID == user.Id).ToList();

            var wordList = appDbContext.Word.Where(x => x.UserID == user.Id).ToList();
            var missingWords = wordList.Where(x => !quizlist.Any(q => q.WordID == x.WordID)).ToList();

            if (missingWords.Count >= 4)
            {
                for (int i = 0; i < count; i++)
                {
                    Random rnd = new Random();
                    int randomIndex = rnd.Next(0, missingWords.Count());
                    var quiz = new Quiz
                    {
                        UserID = user.Id,
                        WordID = missingWords[randomIndex].WordID,
                    };
                    missingWords.Remove(missingWords[randomIndex]);
                    appDbContext.Quiz.Add(quiz);
                }
            }

            else
            {
                Console.WriteLine("4 kelimeden az kelime olduğunu ve yeni kelime eklemek gerektiğini yazdır uyarısını ver");
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
                var tmpword = appDbContext.Word.FirstOrDefault(x => x.WordID == item.WordID);
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
            //Tekrar prensibi koşulları buraya gelecektir
            foreach (var item in quizList)
            {
                TimeSpan? time = DateTime.Now - item.replyDate;

                if (item.counter == 0)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }
                if (item.counter == 1 && time.Value.TotalDays >= 1 && time.Value.TotalDays < 7)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }
                if (item.counter == 2 && time.Value.TotalDays >= 7 && time.Value.TotalDays < 30)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }
                if (item.counter == 3 && time.Value.TotalDays >= 30 && time.Value.TotalDays < 90)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }
                if (item.counter == 4 && time.Value.TotalDays >= 90 && time.Value.TotalDays < 180)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }
                if (item.counter == 5 && time.Value.TotalDays >= 180)
                {
                    TestList.Add(await CreateTestLogEntity(item, quizList));
                }

            }
            return View(TestList);
        }
        public async Task<TestLog> CreateTestLogEntity(Quiz quiz, List<Quiz> quizList)
        {
            var trueWordEN = appDbContext.Word.FirstOrDefault(x => x.WordID == quiz.WordID).wordEN;
            var trueWordTR = appDbContext.Word.FirstOrDefault(x => x.WordID == quiz.WordID).wordTR;
            var trueWordSentences = appDbContext.Word.FirstOrDefault(x => x.WordID == quiz.WordID).wordEN;
            var wrdimg = appDbContext.Word.FirstOrDefault(x => x.WordID == quiz.WordID).wordImage;


            var wrd = appDbContext.Word.FirstOrDefault(x => x.WordID == quiz.WordID);
            var falseWords = await randomFalseAnswer(wrd, quizList);
            var test = new TestLog
            {
                QuizID = quiz.quizID,
                trueWord = trueWordEN,
                TrueWordTR = trueWordTR,
                trueSentences = trueWordSentences,
                WordImage = wrdimg,
                falseWord1 = falseWords[0].wordTR,
                falseWord2 = falseWords[1].wordTR,
                falseWord3 = falseWords[2].wordTR
            };
            return test;

        }

        [HttpPost]
        public async Task<IActionResult> SaveTestLog([FromBody] TestLog data)
        {


            TestLog editedData = data;
            Quiz? quizData = appDbContext.Quiz.FirstOrDefault(x => x.quizID == editedData.QuizID);
            AppUser? user = await userManager.FindByIdAsync(quizData.UserID);
            if (data.TrueWordTR.Trim() == data.SelectedAnswer.Trim())
            {
                editedData.testResult = true;
                quizData.counter++;
                quizData.replyDate = DateTime.Now;
                quizData.level++;
                appDbContext.Quiz.Update(quizData);
            }
            else
            {
                editedData.testResult = false;
                quizData.counter = 0;
                quizData.replyDate = DateTime.Now;
                quizData.level = 0;
                appDbContext.Quiz.Update(quizData);
            }
            editedData.logDate = DateTime.Now;
            appDbContext.TestLog.Add(editedData);
            appDbContext.SaveChanges();
            return Ok();
        }
        public async Task<IActionResult> StartTest()
        {

            //Quiz Var mı yok Mu diye kontrol etmeli ve daha sonra quiz yoksa eğer oluşturmalı
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var quizList = appDbContext.Quiz.Where(w => w.UserID == user.Id).ToList();

            //Eğer Hiç Testi Yoksa
            if (quizList.Count == 0)
            {
                await CreateQuiz();
                return RedirectToAction("StartTest", "Member");
            }
            //Eğer test leveli sıfır olanların sayısı user QuestionCounttan az ise yeni oluşturacak. ve ekleyecek
            if (quizList.Where(x => x.level == 0).ToList().Count < user.quizQuestionCount)
            {
                int newQuizCount = user.quizQuestionCount - quizList.Where(x => x.level == 0 && x.counter == 0).Count();
                await CreateNewQuiz(newQuizCount, quizList);
                return RedirectToAction("StartTest", "Member");
            }
            if (quizList.Where(x => x.level == 0).ToList().Count == 0 && quizList.ToList().Count > 0)
            {
                int newQuizCount = user.quizQuestionCount - quizList.Where(x => x.level == 0 && x.counter == 0).Count();
                if (newQuizCount == 0)
                {
                    await CreateNewQuiz(4, quizList);
                    return RedirectToAction("StartTest", "Member");
                }
                else
                {
                    await CreateNewQuiz(newQuizCount, quizList);
                    return RedirectToAction("StartTest", "Member");
                }

            }

            //Test'e giriş.
            if (quizList.Where(x => x.level == 0).Count() > user.quizQuestionCount)
            {

                string quizListJSON = JsonConvert.SerializeObject(quizList);
                TempData["QuizList"] = quizListJSON;
                return RedirectToAction("SolveQuestion");
            }
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> changeUserFeature()
        {
            var user= await userManager.FindByNameAsync(User.Identity.Name);

            var updateUserViewModel = new UserUpdateViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                UserName = user.UserName,
                quizQuestionCount = user.quizQuestionCount
            };

            return View(updateUserViewModel);
        }



        //Kullanıcı ismi değiştiğinde çıkış yapılacak. ve kullanıcıdan yeniden girilmesi istenecek.
        [HttpPost]
        public async Task<IActionResult> changeUserFeature(UserUpdateViewModel request)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            
            user.Name = request.Name;
            user.Surname = request.Surname;
            user.UserName = request.UserName;
            user.quizQuestionCount = request.quizQuestionCount;
            user.Email = request.Email;
           
            var identityResult = await  userManager.UpdateAsync(user);

            foreach (IdentityError item in identityResult.Errors)
            {
                ModelState.AddModelError(string.Empty, item.Description);
            }

            return View();
        }


        public async Task<IActionResult> myStats()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var TestlogList = appDbContext.TestLog.Where(x => x.Quiz!.UserID == user.Id);
            var quizzes = appDbContext.Quiz.Where(x => x.UserID == user.Id);

            
            List<WordSuccessTableViewModel> tableList = new List<WordSuccessTableViewModel>();


            List<WordSuccessTableViewModel> data = TestlogList.GroupBy(t => t.QuizID).Select(x => new WordSuccessTableViewModel
            {
                WordTR = appDbContext.Quiz.FirstOrDefault(w=>w.quizID==x.Key).Word.wordTR,
                WordEN = appDbContext.Quiz.FirstOrDefault(w=>w.quizID==x.Key).Word.wordEN,
                TrueCount = x.Count(y => y.testResult == true),
                FalseCount = x.Count(y => y.testResult == false)
            }).ToList();


            


            var stats = new UserStatsViewModel
            {
                trueAnswerCount = 7,
                falseAnswerCount = 5
            };


            return View(data);
        }

        public async Task<IActionResult> CreatePDF()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var TestlogList = appDbContext.TestLog.Where(x => x.Quiz!.UserID == user.Id);
            List<WordSuccessTableViewModel> data = TestlogList.GroupBy(t => t.QuizID).Select(x => new WordSuccessTableViewModel
            {
                WordTR = appDbContext.Quiz.FirstOrDefault(w => w.quizID == x.Key).Word.wordTR,
                WordEN = appDbContext.Quiz.FirstOrDefault(w => w.quizID == x.Key).Word.wordEN,
                TrueCount = x.Count(y => y.testResult == true),
                FalseCount = x.Count(y => y.testResult == false)
            }).ToList();


            var pdfstring = @" <style>
    table {
        border-collapse: collapse;
        width: 100%;
    }

    th, td {
        border: 1px solid black;
        padding: 8px;
        text-align: left;
    }

    th {
        background-color: #f2f2f2;
    }
</style> <table><thead>
        <tr>
          <th>&nbsp;</th>
          <th>İngilizce Kelime</th>
          <th>Türkçe Kelime</th>
          <th>Doğru Sayısı</th>
          <th>Yanlış Sayısı</th> 
          <th>Başarı Oranı</th>
        </tr>
    </thead>
<tbody>";

            for (int i = 0; i < data.Count; i++)
            {
                var item = data[i];

                pdfstring += $@"
    <tr>
        <td>{i + 1}</td>
        <td>{item.WordEN}</td>
        <td>{item.WordTR}</td>
        <td>{item.TrueCount}</td>
        <td>{item.FalseCount}</td>
        <td>{((Convert.ToDouble(item.TrueCount)/(Convert.ToDouble(item.TrueCount)+Convert.ToDouble(item.FalseCount)))*100)}</td>

    </tr>";
            }
            pdfstring += "</tbody> </table>";




            var converter = new SynchronizedConverter(new PdfTools());

            var doc = new HtmlToPdfDocument()
            {
                GlobalSettings = {
        ColorMode = ColorMode.Color,
        Orientation = Orientation.Landscape,
        PaperSize = PaperKind.A4Plus,
              },
                Objects = {
          new ObjectSettings() {
             PagesCount = true,
            HtmlContent = pdfstring,
            WebSettings = { DefaultEncoding = "utf-8" },
            HeaderSettings = { FontSize = 9, Right = "Page [page] of [toPage]", Line = true, Spacing = 2.812 }
                 }
                }
            };


            byte[] pdf = converter.Convert(doc);

            return new FileContentResult(pdf, "application/pdf");


        }



        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {



            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            var currentUser = await userManager.FindByNameAsync(User.Identity!.Name!);
            var checkOldPassword = await userManager.CheckPasswordAsync(currentUser, request.oldPassword);
            if (!checkOldPassword)
            {
                ModelState.AddModelErrorList(new List<string> { "Girilen Eski Şifre Hatalı" });
                return View();
            }
            var resultChangePassword = await userManager.ChangePasswordAsync(currentUser, request.oldPassword, request.newPassword);
            if (!resultChangePassword.Succeeded)
            {
                ModelState.AddModelErrorList(resultChangePassword.Errors.Select(x => x.Description).ToList());
                return View();
            }

            await userManager.UpdateSecurityStampAsync(currentUser);
            await signInManager.SignOutAsync();
            await signInManager.PasswordSignInAsync(currentUser, request.newPassword, true, false);
            TempData["SuccessMessage"] = "Şifreniz Başarıyla Değiştirilmiştir.";
            return View();

        }



    }
}