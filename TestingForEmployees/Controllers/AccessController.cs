using System;
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

namespace TestingForEmployees.Controllers
{
    public class AccessController : Controller
    {
        private readonly UserManager<ApplicationUsers> userManager;
        private readonly SignInManager<ApplicationUsers> signInManager;
        private readonly DataContext dataContext;


        public AccessController(UserManager<ApplicationUsers> usermanager, SignInManager<ApplicationUsers> signinmanager, DataContext datacontext)
        {
            userManager = usermanager;
            signInManager = signinmanager;
            dataContext = datacontext;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> UsersTaskForTest(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var userCount = dataContext.TitleUserCountAccess.Include(x => x.Title).Where(x => x.User == user & x.Title.WorkStateTitle == true).ToList();
                    var titleAll = dataContext.TestTitle.Where(x => x.WorkStateTitle == true);
                    // если всего тем в доступе 0 но всего темы есть заполним темы для сдачи 
                    if (userCount.Count() == 0 && titleAll.Count() > 0)
                    {
                        foreach (var itm in titleAll)
                        {
                            dataContext.TitleUserCountAccess.Add(
                                    new TitleUserCountAccess()
                                    {
                                        Title = itm,
                                        User = user,
                                        DateStart = null,
                                        State = false
                                    }
                                );
                        }
                        try
                        {
                            await dataContext.SaveChangesAsync();
                        }
                        catch (Exception)
                        {

                            throw;
                        }

                    }
                    // если же темы в доступе есть "каке-то" но их количество не равно тому что в доступе переберем и добавим тех что нету
                    else if (userCount.Count() > 0 && (userCount.Count() != titleAll.Count()))
                    {
                        foreach (var title in titleAll)
                        {
                            var y = userCount.FirstOrDefault(x => x.Title == title);
                            if (y == null)
                            {
                                dataContext.TitleUserCountAccess.Add(
                                    new TitleUserCountAccess()
                                    {
                                        User = user,
                                        Title = title,
                                        DateStart = null,
                                        State = false
                                    });
                            }
                        }
                        try
                        {
                            await dataContext.SaveChangesAsync();

                        }
                        catch (Exception ex)
                        {

                            throw ex;
                        }
                    }
                    // учтем то что если тема была удалена но в доступе осталась мы ёё не увидем, хотя в базе она будет оставим это для истории
                    var Access = dataContext.TitleUserCountAccess.Where(x => x.User == user).Include(t => t.Title).Where(x => x.Title.WorkStateTitle == true).ToList();

                    if (Access != null && user != null)
                    {

                        var model = new AccessViewModel()
                        {
                            User = user,
                            AccessCollection = Access

                        };
                        return View("UsersTaskForTest", model);
                    }
                }
            }
            return StatusCode(404);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> AccessChange(int? AccessCount, bool? AccessState, string UserId, int? AccessId)
        {
            if (AccessCount != null && AccessState != null && !string.IsNullOrEmpty(UserId) && AccessId != null && AccessId is Int32)
            {
                var accessUnit = await dataContext.TitleUserCountAccess.Include(x => x.Title).SingleOrDefaultAsync(x => x.Id == AccessId && x.User.Id == UserId && x.Title.WorkStateTitle == true);

                if (accessUnit != null)
                {
                    accessUnit.State = (bool)AccessState;
                    accessUnit.DateStart = DateTime.Now;
                    accessUnit.Attempts = (int)AccessCount;
                    await dataContext.SaveChangesAsync();
                    return StatusCode(200);
                }
            }
            return StatusCode(400);
        }
    }
}