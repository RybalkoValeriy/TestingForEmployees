using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TestingForEmployees.Models;
using TestingForEmployees.Models.Entities;
using TestingForEmployees.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using TestingForEmployees.Util;


namespace TestingForEmployees.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly UserManager<ApplicationUsers> userManager;
        private readonly SignInManager<ApplicationUsers> signInManager;
        private readonly DataContext dataContext;


        public HomeController(UserManager<ApplicationUsers> usermanager, SignInManager<ApplicationUsers> signinmanager, DataContext datacontext)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            dataContext = datacontext;
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await userManager.GetUserAsync(User);

            var titlesAccessed = dataContext.TitleUserCountAccess.Include(x => x.Title).ThenInclude(q => q.QuestionsId).ThenInclude(x => x.TestAnswers).Include(s => s.StartedTestLogCollection)
                .Where(
                x => x.User == user &&
                x.State == true &&
                x.DateStart != null &&
                x.Title.CountTestQuestion <= x.Title.QuestionsId.Where(q => q.WorkStateQuestion == true).Count() &&
                x.Title.CountTestQuestion != 0 &&
                x.Title.QuestionsId.Count() > 0 &&
                x.Title.WorkStateTitle == true).ToList();

            List<StartedTitleTestViewModel> stTitleTestViewModel = new List<StartedTitleTestViewModel>();

            //коллекция невалидных тем
            List<TitleUserCountAccess> validate = new List<TitleUserCountAccess>();

            foreach (var item in titlesAccessed)
            {
                // проверим все ли ответы заполнены 
                foreach (var question in item.Title.QuestionsId)
                {
                    if (question.TestAnswers.Where(x => x.WorkStateAnswers == true).Count() == 0)
                    {
                        //добавим в коллекцию невалидных тем
                        validate.Add(item);
                    }
                }

                // проверим что бы не была в коллекции невалидных тем 
                if (!validate.Contains(item))
                {
                    // проверяем тест начат ли сегодня
                    var validStarted = item.StartedTestLogCollection.FirstOrDefault(x => x.DateStarted.Date == DateTime.Now.Date);

                    // тест уже был начат сегодня но не закончин
                    if (validStarted != null && validStarted.State == true)
                    {
                     var allquest=dataContext.StartedTestLog.Include(x => x.FactTitleLogCollection).ThenInclude(x => x.FactQuestCollection).Include(x => x.TitleUserAccess).FirstOrDefault(x => x.UserId == user && x.DateStarted.Date == DateTime.Now.Date && x.TitleUserAccess.Title == item.Title).FactTitleLogCollection.Select(x=>x.FactQuestCollection.Count()).ToArray();

                        var allansw = dataContext.AnswerUserResultLog.Include(x => x.factQuestLog).Include(x => x.factTitleLog).ThenInclude(x => x.StartedTestLog).ThenInclude(x => x.TitleUserAccess).Where(x => x.factTitleLog.StartedTestLog.UserId == user && x.factTitleLog.StartedTestLog.TitleUserAccess.Title == item.Title && x.factTitleLog.StartedTestLog.State==true&&x.DateAnsw.Date==DateTime.Now.Date).Select(x=>x.factQuestLog).Distinct().Count(); 

                        stTitleTestViewModel.Add(new StartedTitleTestViewModel()
                        {
                            IdTitle = item.Title.Id,
                            Title = item.Title.Title,
                            CountQuestion = allquest[0]-allansw, // сколько соталось все вопросы-вопросы на которые уже дан ответ
                            DateStarted = validStarted.DateStarted,
                            State = true,
                            StartedTestLog = validStarted
                        });
                    }
                    // тест закончен
                    else if (validStarted != null && validStarted.State == false)
                    {
                        stTitleTestViewModel.Add(new StartedTitleTestViewModel()
                        {
                            IdTitle = item.Title.Id,
                            Title = item.Title.Title,
                            CountQuestion = dataContext.FactQuestLog.Include(x => x.FactTitleLog).ThenInclude(x => x.StartedTestLog).ThenInclude(x => x.FactTitleLogCollection).Where(x => x.DateAdd.Date == DateTime.Now.Date && x.FactTitleLog.StartedTestLog.TitleUserAccess.Title == item.Title && x.FactTitleLog.StartedTestLog.UserId == item.User).Count(),
                            DateStarted = validStarted.DateStarted,
                            State = false,
                            StartedTestLog = validStarted
                        });
                    }
                    // тест не был начат и варианты ответов есть
                    else if (validStarted == null && item.Attempts > 0)
                    {
                        stTitleTestViewModel.Add(new StartedTitleTestViewModel()
                        {
                            IdTitle = item.Title.Id,
                            Title = item.Title.Title,
                            CountQuestion = item.Title.CountTestQuestion,
                            DateStarted = null,
                            State = false,
                            StartedTestLog = validStarted
                        });
                    }
                }
            }
            return View("Index", stTitleTestViewModel);
        }



        public async Task<IActionResult> New(int? TestId)
        {
            var user = await userManager.GetUserAsync(User);
            // найдем и провалидируем этот тест
            var access = await dataContext.TitleUserCountAccess.Include(x => x.Title).ThenInclude(q => q.QuestionsId).Include(s => s.StartedTestLogCollection)
                .Where(
                x => x.User == user &&
                x.State == true &&
                x.DateStart is DateTime &&
                x.Attempts > 0 &&
                x.Title.CountTestQuestion <= x.Title.QuestionsId.Where(q => q.WorkStateQuestion == true).Count() &&
                x.Title.CountTestQuestion != 0 &&
                x.Title.QuestionsId.Count() > 0 &&
                x.Title.WorkStateTitle == true).FirstOrDefaultAsync(x => x.Title.Id == TestId);

            // смотрим начали ли мы уже этот тест сегодня
            var continueTest = await dataContext.StartedTestLog.FirstOrDefaultAsync(st => st.UserId == user && st.State == true && st.DateStarted.Date == DateTime.Now.Date && st.TitleUserAccess.Title.Id == TestId);

            //доступ есть и тест уже начали сегодня
            if (continueTest != null && access != null)
            {
                return RedirectToAction("Continue", "Home", new { StLg = continueTest.Id });
            }
            // есть доступ и тест ещё не начат сегодня
            else if (access != null && continueTest == null)
            {
                // все вопросы по даной теме
                var idAll = dataContext.TestQuestions.Where(x => x.Title == access.Title && x.WorkStateQuestion == true).Select(x => x.Id);

                // выбираем случайны-уникальный список id-вопросов из всех
                var IdQuestRandom = RandomQuestions.ReturnRandomUnique(access.Title.CountTestQuestion, idAll.ToList());

                // полученный список вопросов случайный-уникальный и захватим ответы
                var Questions = dataContext.TestQuestions.Include(x => x.Title).
                    Where(x => IdQuestRandom.Any(r => r == x.Id)).Where(x => x.Title == access.Title && x.WorkStateQuestion == true).Include(x => x.TestAnswers).OrderBy(x => x.Id);

                List<TestQuestions> QuestionsRandomUnique = new List<TestQuestions>();

                foreach (var id in IdQuestRandom)
                {
                    QuestionsRandomUnique.Add(await Questions.FirstOrDefaultAsync(x => x.Id == id));
                }

                #region startedLog
                // создаем запись старт сдачи теста 
                var startedLog = new StartedTestLog()
                {
                    UserId = user,
                    State = true,
                    TitleUserAccess = access,
                    IP = Request.HttpContext.Connection.RemoteIpAddress.ToString()
                    //todo: ip-addres
                };

                await dataContext.StartedTestLog.AddAsync(startedLog);
                #endregion

                #region factTitle
                // создаем запись с темой для старта сегодня
                var factTitle = new FactTitleLog()
                {
                    StartedTestLog = startedLog,
                    TitleText = access.Title.Title,
                };

                await dataContext.FactTitleLog.AddAsync(factTitle);
                #endregion

                #region factQuest
                // запись с вопросами на текущий момент
                ICollection<FactQuestLog> factQuest = new List<FactQuestLog>();
                foreach (var item in QuestionsRandomUnique)
                {
                    if (item.WorkStateQuestion == true)
                    {
                        factQuest.Add(new FactQuestLog()
                        {
                            FactTitleLog = factTitle,
                            QuestText = item.Questions
                        });
                    }

                }
                await dataContext.FactQuestLog.AddRangeAsync(factQuest);
                #endregion

                #region factAnswer
                // запись с ответами на текущий момент
                ICollection<FactAnswersLog> factAnswer = new List<FactAnswersLog>();
                foreach (var answers in QuestionsRandomUnique.Select(a => a.TestAnswers))
                {
                    foreach (var answ in answers)
                    {
                        if (answ.WorkStateAnswers == true)
                        {
                            factAnswer.Add(new FactAnswersLog()
                            {
                                AnswersText = answ.Title,
                                FactQuestLog = factQuest.FirstOrDefault(x => x.QuestText == answ.TestQuestions.Questions),
                                State = answ.State
                            });
                        }
                    }
                }
                await dataContext.FactAnswersLog.AddRangeAsync(factAnswer);
                #endregion

                // уменьшаем попытки
                if (access.Attempts > 0)
                {
                    access.Attempts = access.Attempts - 1;
                    dataContext.TitleUserCountAccess.Update(access);
                }
                await dataContext.SaveChangesAsync();
                return RedirectToAction("Continue", "Home", new { StLg = startedLog.Id });
            }
            return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Results(int? StLg)
        {
            if (StLg != null && StLg is int)
            {
                var user = await userManager.GetUserAsync(User);
                // проверим есть ли результат
                var result = await dataContext.ResultReports.FirstOrDefaultAsync(x => x.StartedTestLog.Id == StLg && x.User == user && x.DateAnsw.Date == DateTime.Now.Date);

                // добавим если резульат не записан
                if (result == null)
                {
                    var fact = await dataContext.FactTitleLog.Include(x => x.StartedTestLog).ThenInclude(x => x.TitleUserAccess).Include(m => m.FactQuestCollection).ThenInclude(a => a.FactAnswersLog).Include(x => x.AnswerUserResultLog).FirstOrDefaultAsync(x => x.DateAdd.Date == DateTime.Now.Date && x.StartedTestLog.Id == StLg && x.StartedTestLog.UserId == user);
                    if (fact != null)
                    {
                        var uAnswer = fact.AnswerUserResultLog.Where(x => x.User == user).Count();
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
                                        var fansw = AnswerResult.factQuestLog.FactAnswersLog;
                                        var uansw = AnswerResult.factQuestLog.AnswerUserResultLog;
                                        // сравним кол-во ответов
                                        //if (fansw.Count() == uansw.Count())
                                        //{
                                        //    var fanswState = fansw.Select(x => x.State).ToList();
                                        //    var uanswState = uansw.Select(x => x.State).ToList();
                                        //    for (int i = 0; i < fanswState.Count(); i++)
                                        //    {
                                        //        if (fanswState[i] != uanswState[i])
                                        //        {
                                        //            flag = false;
                                        //            break;
                                        //        }
                                        //    }
                                        //    if (flag)
                                        //    {
                                        //        if (!trueAnsw.Contains(AnswerResult.factQuestLog))
                                        //        {
                                        //            trueAnsw.Add(AnswerResult.factQuestLog);
                                        //        }
                                        //    }
                                        //}
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
                                    }
                                }
                            }

                            ResultReport report = new ResultReport
                            {
                                User = user,
                                FactTitleLog = fact,
                                StartedTestLog = fact.StartedTestLog,
                                Percent = (trueAnsw.Count() * 100) / fact.FactQuestCollection.Count()
                            };

                            await dataContext.ResultReports.AddAsync(report);
                            await dataContext.SaveChangesAsync();
                            return View("Result", report.Percent);
                        }
                    }
                }
                else
                {
                    return View("Result", result.Percent);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Continue(int? StLg)
        {
            if (StLg != null && StLg is Int32)
            {
                var user = await userManager.GetUserAsync(User);
                var started = await dataContext.StartedTestLog.Include(t => t.FactTitleLogCollection).Where(x => x.Id == StLg && x.UserId == user && x.DateStarted.Date == DateTime.Now.Date && x.State == true).FirstOrDefaultAsync();

                if (started != null)
                {
                    // получим текущие вопросы для темы из лога 
                    var data = await dataContext.FactTitleLog.Include(x => x.StartedTestLog).ThenInclude(s => s.TitleUserAccess).Include(q => q.FactQuestCollection).ThenInclude(a => a.FactAnswersLog).ThenInclude(answ => answ.AnswerUserResultLog).FirstOrDefaultAsync(x => x.StartedTestLog == started);

                    // уже есть ответы по этому тесты в базе
                    if (data.AnswerUserResultLog != null)
                    {
                        // тест уже закончен
                        if (data.AnswerUserResultLog.Where(x => x.factQuestLog.FactTitleLog.StartedTestLog.Id == StLg).Count() == data.FactQuestCollection.Select(x => x.FactAnswersLog.Count()).Sum())
                        {
                            data.StartedTestLog.State = false;
                            dataContext.StartedTestLog.Update(data.StartedTestLog);
                            await dataContext.SaveChangesAsync();
                            return RedirectToAction("Results", new { StLg = data.StartedTestLog.Id });
                        }
                        // продолжим сдачу
                        else
                        {
                            PassQuestionViewModel model = new PassQuestionViewModel
                            {
                                startedTestLog = data.StartedTestLog,
                                factTitleLog = data,
                                factQuestLog = data.FactQuestCollection.Where(x => x.AnswerUserResultLog == null && x.DateAdd.Date == DateTime.Now.Date).FirstOrDefault(),
                                factAnswersLog = data.FactQuestCollection.Where(x => x.AnswerUserResultLog == null && x.DateAdd.Date == DateTime.Now.Date).FirstOrDefault().FactAnswersLog
                            };
                            return View("Continue", model);
                        }
                    }
                    // дадим первый ответ
                    else
                    {
                        PassQuestionViewModel model = new PassQuestionViewModel
                        {
                            startedTestLog = data.StartedTestLog,
                            factTitleLog = data,
                            factQuestLog = data.FactQuestCollection.FirstOrDefault(),
                            factAnswersLog = data.FactQuestCollection.FirstOrDefault().FactAnswersLog
                        };
                        return View("Continue", model);
                    }
                }
            }
            return View("Index");
        }




        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> SaveUserResponse(int? StLg, int? fTitle, int? fQuest, int[] ansId)
        {
            if (StLg != null && StLg is Int32 &&
                fTitle != null && fTitle is Int32 &&
                fQuest != null && fQuest is Int32)
            {
                var user = await userManager.GetUserAsync(User);

                var fact = await dataContext.FactTitleLog.Include(fq => fq.FactQuestCollection).ThenInclude(fa => fa.FactAnswersLog).Include(st => st.StartedTestLog).ThenInclude(tAcc => tAcc.TitleUserAccess).Include(res => res.AnswerUserResultLog).FirstOrDefaultAsync(x => x.Id == fTitle && x.StartedTestLog.Id == StLg && x.FactQuestCollection.FirstOrDefault(q => q.Id == fQuest).Id == fQuest && x.DateAdd.Date == DateTime.Now.Date && x.StartedTestLog.UserId == user);

                // коллекция ответов
                List<AnswerUserResultLog> answLog = new List<AnswerUserResultLog>();

                if (fact != null)
                {
                    // все ответы false
                    if (ansId.Length == 0)
                    {
                        // даем первый ответ
                        if (fact.AnswerUserResultLog.Count() == 0)
                        {
                            foreach (var item in fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.Where(a => a.DateAdd.Date == DateTime.Now.Date))
                            {
                                if (item.AnswerUserResultLog == null)
                                {
                                    answLog.Add(new AnswerUserResultLog()
                                    {
                                        User = user,
                                        State = false,
                                        factTitleLog = fact,
                                        factQuestLog = item.FactQuestLog,
                                        factAnswersLog = item
                                    });
                                }
                            }

                            if (answLog.Count() > 0)
                            {
                                dataContext.AnswerUserResultLog.AddRange(answLog);
                                await dataContext.SaveChangesAsync();
                            }

                            return RedirectToAction("Continue", new { StLg = fact.StartedTestLog.Id });
                        }

                        // не первый овет
                        else if (fact.AnswerUserResultLog.Count() > 0)
                        {
                            foreach (var item in fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.Where(a => a.DateAdd.Date == DateTime.Now.Date && a.AnswerUserResultLog == null))
                            {
                                answLog.Add(new AnswerUserResultLog()
                                {
                                    User = user,
                                    State = false,
                                    factTitleLog = fact,
                                    factQuestLog = item.FactQuestLog,
                                    factAnswersLog = item
                                });
                            }
                            if (answLog.Count() > 0)
                            {
                                dataContext.AnswerUserResultLog.AddRange(answLog);
                                await dataContext.SaveChangesAsync();
                            }

                            return RedirectToAction("Continue", new { StLg = fact.StartedTestLog.Id });
                        }
                    }
                    // смотри что пользователь дал некоторые ответы true так же проверим что бы этих ответов не было больше чем ответов всего
                    else if (ansId.Count() > 0 && ansId.Count() <= fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.Where(a => a.DateAdd.Date == DateTime.Now.Date).Count())
                    {
                        // все ответы в базе
                        foreach (var item in fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.Where(a => a.DateAdd.Date == DateTime.Now.Date && a.AnswerUserResultLog == null))
                        {
                            // ответы true пользователя
                            foreach (var id in ansId)
                            {
                                if (item.Id == id)
                                {
                                    answLog.Add(new AnswerUserResultLog()
                                    {
                                        User = user,
                                        State = true,
                                        factTitleLog = fact,
                                        factQuestLog = item.FactQuestLog,
                                        factAnswersLog = fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.FirstOrDefault(x => x.Id == id)
                                    });
                                }
                            }
                            if (!answLog.Any(a => a.factAnswersLog.Id == item.Id))
                            {
                                answLog.Add(new AnswerUserResultLog()
                                {
                                    User = user,
                                    State = false,
                                    factTitleLog = fact,
                                    factQuestLog = item.FactQuestLog,
                                    factAnswersLog = fact.FactQuestCollection.FirstOrDefault(x => x.Id == fQuest && x.DateAdd.Date == DateTime.Now.Date).FactAnswersLog.FirstOrDefault(x => x == item)
                                });
                            }
                        }
                        if (answLog.Count() > 0)
                        {
                            dataContext.AnswerUserResultLog.AddRange(answLog);
                            await dataContext.SaveChangesAsync();
                        }
                        return RedirectToAction("Continue", new { StLg = fact.StartedTestLog.Id });
                    }
                }
            }
            return View("Index");
        }
    }
}
