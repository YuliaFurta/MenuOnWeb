using MenuOnWebFinal.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MenuOnWebFinal.Controllers
{
    public class UserController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Edit()
        {
            var curUser = User.Identity;
            var user = context.Users.Find(curUser.GetUserId());
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Login = user.Login,
                PhoneNumber = user.PhoneNumber,
                ImageFile = null
            });
        }

        public string UploadFile(HttpPostedFileBase file, string pathPart)
        {
            if (file == null) return string.Empty;

            var fileName = Path.GetFileName(file.FileName);
            var random = Guid.NewGuid() + fileName;
            var path = Path.Combine(HttpContext.Server.MapPath("~/Content/" + pathPart), random);
            if (!Directory.Exists(HttpContext.Server.MapPath("~/Content/" + pathPart)))
            {
                Directory.CreateDirectory(HttpContext.Server.MapPath("~/Content/" + pathPart));
            }
            file.SaveAs(path);

            return random;
        }

        [HttpPost]
        public ActionResult Edit(EditUserViewModel editedUser)
        { 
                var userToUpdate = context.Users.Find(editedUser.Id);

                string imagePath = "";
                string imageToDelete = "";
            if (editedUser.ImageFile != null)
            {
                imagePath = UploadFile(editedUser.ImageFile, "images/thumbnails");
                if (userToUpdate.AvatarUrl != null)
                {
                    imageToDelete = userToUpdate.AvatarUrl;
                }
            }

                userToUpdate.Email = editedUser.Email;
                userToUpdate.Login = editedUser.Login;
                userToUpdate.PhoneNumber = editedUser.PhoneNumber;

                if (imagePath != "")
                {
                    userToUpdate.AvatarUrl = imagePath;
                }

                context.Entry(userToUpdate).State = EntityState.Modified;
                context.SaveChanges();

                if (imageToDelete != "")
                {
                    System.IO.File.Delete(HttpContext.Server.MapPath("~/Content/images/thumbnails/" + imageToDelete));
                }

                return RedirectToAction("Index", "Home");
        }

    }
}