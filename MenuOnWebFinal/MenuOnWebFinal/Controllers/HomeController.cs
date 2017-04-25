using MenuOnWebFinal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MenuOnWebFinal.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            List<ViewRecipe> recipes = new List<ViewRecipe>();
           
                foreach (var item in context.Recipes)
                {
                    recipes.Add((ViewRecipe)item);
                }

            ViewBag.Recipes = recipes;

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.IsAuthenticated = "true";
                var user = User.Identity;
                var userManager = new UserManager<User>(new UserStore<User>(context));
                var usersRoles = userManager.GetRoles(user.GetUserId()).ToList();
                usersRoles.Sort();

                if(usersRoles[0] == "Administrator")
                {
                    ViewBag.EnableAction = "links";
                }
            }
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Authorized user.";

            return View();
        }
    }
}