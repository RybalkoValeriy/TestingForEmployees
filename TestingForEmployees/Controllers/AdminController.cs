using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using TestingForEmployees.Models;
using TestingForEmployees.Models.Entities;
using TestingForEmployees.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;





namespace TestingForEmployees.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUsers> userManager;
        private readonly SignInManager<ApplicationUsers> signInManager;
        private readonly DataContext dataContext;
        private readonly RoleManager<IdentityRole> roleManager;
        public AdminController(UserManager<ApplicationUsers> usermanager, SignInManager<ApplicationUsers> signinmanager, DataContext datacontext, RoleManager<IdentityRole> roleManager)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            dataContext = datacontext;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UsersControl()
        {
            //var users = await userManager.GetUsersInRoleAsync("user");

            var user = dataContext.Users.Where(x => x.UserName != "adminadm").Include(x => x.Branch).ToList();
            return View("UsersControl",user);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string userId, string newPass)
        {
            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(newPass))
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var resultRemove = await userManager.RemovePasswordAsync(user);
                    if (resultRemove.Succeeded)
                    {
                        var resultNew = await userManager.AddPasswordAsync(user, newPass);
                        if (resultNew.Succeeded)
                        {
                            return StatusCode(200, "Пароль успішно змінено");
                        }
                        return StatusCode(400, "Пароль старий видалено, але новий не задано");
                    }
                    return StatusCode(400, "Пароль старий не видалено");
                }
            }
            return StatusCode(400, "Помилкові дані");
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult TitleTest()
        {
            var title = dataContext.TestTitle.Where(x => x.WorkStateTitle == true).Include(q => q.QuestionsId).OrderByDescending(x => x.DateAdd);

            return View("TitleTest", title);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewTitle(string newTitle)
        {
            if (!string.IsNullOrEmpty(newTitle))
            {
                var testTitle = dataContext.TestTitle.Add(
                    new TestTitle() { Title = newTitle }
                    );

                var result = await dataContext.SaveChangesAsync();
                if (result == 1)
                {
                    var Title = dataContext.TestTitle.Where(x => x.WorkStateTitle == true).Include(x => x.QuestionsId).OrderByDescending(x => x.DateAdd).Select(a => new { a.DateAdd, a.Id, a.Title, a.CountTestQuestion, QuestCount = a.QuestionsId.Where(x => x.WorkStateQuestion == true).Count() }).ToList();
                    return Json(Title);
                }
                return StatusCode(400);
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteTitle(int? id)
        {
            if (id != null)
            {
                var findIdTitle = await dataContext.TestTitle.FindAsync(id);

                if (findIdTitle != null && findIdTitle.WorkStateTitle == true)
                {
                    // статус работы убираю тема-вопросы-ответы
                    findIdTitle.WorkStateTitle = false;

                    if (findIdTitle.QuestionsId != null)
                    {
                        foreach (var quest in findIdTitle.QuestionsId)
                        {
                            quest.WorkStateQuestion = false;
                            quest.DateCloseQuestion = DateTime.Now;
                            if (quest.TestAnswers != null)
                            {
                                foreach (var answ in quest.TestAnswers)
                                {
                                    answ.WorkStateAnswers = false;
                                    answ.DateCloseAnswers = DateTime.Now;
                                }
                            }
                        }
                    }

                    await dataContext.SaveChangesAsync();
                    var Title = dataContext.TestTitle.Where(x => x.WorkStateTitle == true).Include(x => x.QuestionsId).OrderByDescending(x => x.DateAdd).Select(a => new { a.DateAdd, a.Id, a.Title, a.CountTestQuestion, QuestCount = a.QuestionsId.Where(x => x.WorkStateQuestion == true).Count() }).AsNoTracking().ToList();
                    return Json(Title);
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditTitle(string EditTitle, int? id, int? CountTestQue)
        {
            if (!string.IsNullOrEmpty(EditTitle) && id != null && CountTestQue != null)
            {
                var findTitle = await dataContext.TestTitle.FindAsync(id);
                if (findTitle != null && findTitle.WorkStateTitle == true)
                {
                    findTitle.Title = EditTitle.Trim();
                    findTitle.CountTestQuestion = (int)CountTestQue;
                    dataContext.TestTitle.Update(findTitle);
                    await dataContext.SaveChangesAsync();

                    var Title = dataContext.TestTitle.Where(x => x.WorkStateTitle == true).Include(x => x.QuestionsId).OrderByDescending(x => x.DateAdd).Select(a => new { a.DateAdd, a.Id, a.Title, a.CountTestQuestion, QuestCount = a.QuestionsId.Where(x => x.WorkStateQuestion == true).Count() }).ToList();
                    return Json(Title);
                }
            }
            return Json(null);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Question()
        {
            ViewBag.Title = new SelectList(dataContext.TestTitle.Where(x => x.WorkStateTitle == true).OrderByDescending(x => x.DateAdd), "Id", "Title", 10);
            return View("QuestionTest");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult GetGuestion(int? idTitle)
        {
            if (idTitle != null)
            {
                var quetions = dataContext.TestQuestions.Where(x => x.Title.Id == idTitle && x.WorkStateQuestion == true).OrderByDescending(x => x.DateAdd).ToList();
                return Json(quetions);
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddQuestionsToTitle([FromBody]UpdateQuestionsToTitleModel data)
        {
            if (data != null)
            {
                var findTitle = await dataContext.TestTitle.FindAsync(data.IdTitle);
                if (findTitle != null && findTitle.WorkStateTitle == true)
                {
                    foreach (var tq in data.Questions)
                    {
                        await dataContext.TestQuestions.AddAsync(new TestQuestions()
                        {
                            Title = findTitle,
                            Questions = tq,
                            DateAdd = DateTime.Now
                        });
                    }
                    var result = await dataContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var questColl = dataContext.TestQuestions.AsNoTracking().Where(x => x.Title == findTitle && x.WorkStateQuestion == true).OrderByDescending(x => x.DateAdd);
                        return Json(questColl);
                    }
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteQuest(int? idQuest, int? idTitle)
        {
            if (idQuest != null && idTitle != null)
            {
                var quest = await dataContext.TestQuestions.FindAsync(idQuest);
                if (quest != null && quest.WorkStateQuestion == true)
                {
                    quest.WorkStateQuestion = false;
                    quest.DateCloseQuestion = DateTime.Now;

                    if (quest.TestAnswers != null)
                    {
                        foreach (var answ in quest.TestAnswers)
                        {
                            answ.WorkStateAnswers = false;
                            answ.DateCloseAnswers = DateTime.Now;
                        }
                    }

                    await dataContext.SaveChangesAsync();
                }

                var questColl = dataContext.TestQuestions.AsNoTracking().Where(x => x.Title.Id == idTitle && x.WorkStateQuestion == true).OrderByDescending(x => x.DateAdd);
                return Json(questColl);
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> EditQuestion(int? idQuest, int? idTitle, string EditQuest)
        {
            if (idQuest != null && !string.IsNullOrEmpty(EditQuest) && idTitle != null)
            {
                var quest = await dataContext.TestQuestions.FindAsync(idQuest);
                if (quest != null && quest.WorkStateQuestion == true)
                {
                    quest.Questions = EditQuest;
                    dataContext.TestQuestions.Update(quest);
                    var result = await dataContext.SaveChangesAsync();
                    if (result > 0)
                    {
                        var questColl = dataContext.TestQuestions.AsNoTracking().Where(x => x.Title.Id == idTitle && x.WorkStateQuestion == true).OrderByDescending(x => x.DateAdd);
                        return Json(questColl);
                    }
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> GetQuestion(int? idQuest)
        {
            if (idQuest != null)
            {
                var quest = await dataContext.TestQuestions.FindAsync(idQuest);
                if (quest != null && quest.WorkStateQuestion == true)
                {
                    var answer = dataContext.TestAnswers.Where(x => x.TestQuestions == quest && x.WorkStateAnswers == true).AsNoTracking();
                    if (answer != null)
                    {
                        return Json(answer);
                    }
                }
            }
            return StatusCode(400);
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AddAnswers([FromBody]UpdateAnswersToQuestionsModel data)
        {
            if (data != null)
            {
                var findQuest = await dataContext.TestQuestions.FirstOrDefaultAsync(x => x.Id == data.IdQue);

                if (findQuest != null && findQuest.WorkStateQuestion == true)
                {
                    var deletedAnswers = dataContext.TestAnswers.Where(x => x.TestQuestions == findQuest && x.WorkStateAnswers == true);

                    foreach (var item in deletedAnswers)
                    {
                        item.WorkStateAnswers = false;
                        item.DateCloseAnswers = DateTime.Now;
                    }


                    foreach (var itm in data.CollAnswers)
                    {
                        if (!string.IsNullOrEmpty(itm.Answers.Trim()) && itm.State != null)
                        {
                            await dataContext.TestAnswers.AddAsync(
                                    new TestAnswers()
                                    {
                                        State = (bool)itm.State,
                                        TestQuestions = findQuest,
                                        Title = itm.Answers
                                    });
                        }
                    }

                    await dataContext.SaveChangesAsync();

                    return StatusCode(200);
                }
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult Result()
        {
            return View("Result");
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult PostResult(string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                var dt = Convert.ToDateTime(date);

                var result = dataContext.ResultReports.Where(x => x.DateAnsw.Date == dt).Select(x => new { firstName = x.User.FirstName, lastName = x.User.LastName, middleName = x.User.MiddleName, percent = x.Percent, date = x.DateAnsw.TimeOfDay, ResUsr = x.Id.ToString() });
                return Json(result);
            }
            return StatusCode(400);
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public IActionResult ResultDetail(int? ResUsr)
        {
            if (ResUsr != null && ResUsr is int)
            {
                var res = dataContext.ResultReports.Include(x => x.FactTitleLog).FirstOrDefault(x => x.Id == ResUsr);

                var fact = dataContext.FactTitleLog.Include(x => x.StartedTestLog).ThenInclude(x => x.TitleUserAccess).Include(m => m.FactQuestCollection).ThenInclude(a => a.FactAnswersLog).Include(x => x.AnswerUserResultLog).FirstOrDefault(x => x == res.FactTitleLog);
                if (fact != null)
                {
                    var uAnswer = fact.AnswerUserResultLog.Where(x => x.User == res.User).Count();
                    var fAnswer = fact.FactQuestCollection.Select(x => x.FactAnswersLog.Count()).Sum();
                    var r = fact.FactQuestCollection.Select(x => x.FactAnswersLog);

                    if (uAnswer == fAnswer)
                    {
                        // коллекция правильных ответов
                        ICollection<FactAnswersLog> answColl = new List<FactAnswersLog>();
                        foreach (var quest in fact.FactQuestCollection)
                        {
                            foreach (var answ in quest.FactAnswersLog)
                            {
                                answColl.Add(answ);
                            }
                        }

                        List<FactQuestLog> trueAnsw = new List<FactQuestLog>();
                        foreach (var AnswerResult in fact.AnswerUserResultLog)
                        {
                            foreach (var factAnswerLog in answColl)
                            {
                                // сравним вопросы
                                if (AnswerResult.factQuestLog == factAnswerLog.FactQuestLog)
                                {
                                    var flag = true;
                                    var fansw = AnswerResult.factQuestLog.FactAnswersLog.ToList();
                                    var uansw = AnswerResult.factQuestLog.AnswerUserResultLog.ToList();
                                    // сравним кол-во ответов
                                    if (fansw.Count() == uansw.Count())
                                    {
                                        //var fanswState = fansw.Select(x => x.State).ToList();
                                        //var uanswState = uansw.Select(x => x.State).ToList();

                                        foreach (var userFactAnwers in factAnswerLog.FactQuestLog.FactAnswersLog)
                                        {
                                            var answUser = AnswerResult.factQuestLog.AnswerUserResultLog.FirstOrDefault(x => x.factAnswersLog == userFactAnwers);
                                            if (userFactAnwers == answUser.factAnswersLog)
                                            {
                                                if (userFactAnwers.State != answUser.State)
                                                {
                                                    flag = false;
                                                    break;
                                                }
                                            }
                                        }
                                        if (flag)
                                        {
                                            if (!trueAnsw.Contains(AnswerResult.factQuestLog))
                                            {
                                                trueAnsw.Add(AnswerResult.factQuestLog);
                                            }
                                        }

                                        var factAnswer = factAnswerLog.FactQuestLog.FactAnswersLog;
                                        var userAnswer = AnswerResult.factQuestLog.FactAnswersLog;
                                    }
                                }
                            }
                        }
                        // для обнуления процентов
                        if (res.Percent == 0)
                        {
                            res.Percent = trueAnsw.Count() * 100 / fact.FactQuestCollection.Count;
                        }
                        dataContext.Update(res);
                        dataContext.SaveChanges();
                        ResultDetailViewModel detail = new ResultDetailViewModel()
                        {
                            AnswerUserResultLog = fact.AnswerUserResultLog,
                            factAnswersLog = answColl,
                            AppUser = res.User,
                            factTitle = fact,
                            trueQuest = trueAnsw,
                            Percent = res.Percent
                        };
                        return View("Detail", detail);
                    }
                }
            }
            return View("Result");
        }
    }
}