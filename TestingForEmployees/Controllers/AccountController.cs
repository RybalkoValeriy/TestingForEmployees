using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TestingForEmployees.Models;
using TestingForEmployees.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;

namespace TestingForEmployees.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUsers> userManager;
        private readonly SignInManager<ApplicationUsers> signInManager;
        private readonly DataContext dataContext;

        public AccountController(UserManager<ApplicationUsers> usermanager, SignInManager<ApplicationUsers> signinmanager, DataContext datacontext)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            dataContext = datacontext;
        }

        [HttpGet]
        public IActionResult Registration()
        {
            var branch = dataContext.Branches;
            ViewBag.Branch = new SelectList(branch, "Id", "BranchNumber");
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Registration(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                var LastNam = dataContext.Users.FirstOrDefault
                (usr =>
                usr.FirstName == model.FirstName &&
                usr.LastName == model.LastName &&
                usr.MiddleName == model.MiddleName
                );

                var resultValid = await userManager.FindByNameAsync(model.LoginUser);
                if (resultValid != null && resultValid.UserName == model.LoginUser)
                {
                    ModelState.AddModelError(string.Empty, "Користувач з таким логіном вже існує, змініть логін на інший");
                }
                if (LastNam != null)
                {
                    ModelState.AddModelError(string.Empty, "Користувач з таким ФІО вже існує");
                }
                else
                {
                    var branch = dataContext.Branches.Find(model.IDBranch);
                    if (branch != null)
                    {
                        ApplicationUsers user = new ApplicationUsers
                        {
                            Branch = branch,
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            MiddleName = model.MiddleName,
                            UserName = model.LoginUser
                        };

                        var result = await userManager.CreateAsync(user, model.Password);
                        if (result.Succeeded)
                        {
                            var useraddrole = await userManager.AddToRoleAsync(user, "user");
                            if (result.Succeeded)
                            {
                                var claim = new Claim("fio",
                                $"{user.LastName} {user.FirstName} {user.MiddleName}");

                                await userManager.AddClaimAsync(user, claim);

                                await signInManager.SignInAsync(user, false);

                                // Добавим доступ для тестов
                                var testCollection = dataContext.TestTitle;
                                foreach (var itm in testCollection)
                                {
                                    dataContext.TitleUserCountAccess.Add(
                                            new Models.Entities.TitleUserCountAccess()
                                            {
                                                Title = itm,
                                                User = user,
                                                DateStart = null,
                                                Attempts = 3,
                                                State = false
                                            }
                                        );
                                }
                                await dataContext.SaveChangesAsync();

                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(string.Empty, error.Description);
                            }
                        }
                    }
                }
            }
            var branchList = dataContext.Branches;
            ViewBag.Branch = new SelectList(branchList, "Id", "BranchNumber");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Login, model.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(model.Login);
                    if (user != null)
                    {
                        await signInManager.SignInAsync(user, true);
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Невірний логін чи пароль, спробуйте ще або зверніться до адміністратора");
                }
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            if (User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}