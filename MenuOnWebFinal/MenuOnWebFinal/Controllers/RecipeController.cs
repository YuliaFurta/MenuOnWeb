using MenuOnWebFinal.Models;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace MenuOnWebFinal.Controllers
{
    public class RecipeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Recipe/Details/5
        public ActionResult Details(int id)
        {
            var recipe = db.Recipes.FirstOrDefault(i => i.Id == id);
            if (recipe == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Comments = db.Comments.Where(i => i.RecipeId == id);
            return View((ViewRecipe)recipe);
        }

        // GET: Recipe/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(RecipeAddModel model)
        {
            if (ModelState.IsValid)
            {
                Recipe recipe = new Recipe
                {
                    Name = model.Name,
                    Text = model.Text,
                    CreateDate = DateTime.Now,
                    Tags = model.IngridietsString,
                    UserId = User.Identity.GetUserId(),
                };
                string imagePath = UploadFile(model.ImageFile, "images/thumbnails");
                recipe.ImageUrl = imagePath;

                db.Recipes.Add(recipe);
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
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

        // GET: Recipe/Edit/5
        public ActionResult Edit(int id)
        {
            var recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(new RecipeAddModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Text = recipe.Text,
                UserId = User.Identity.GetUserId(),
                IngridietsString = recipe.Tags,
                ImageFile = null
            });
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        public ActionResult Edit(RecipeAddModel editedRecipe)
        {
            try
            {
                if (editedRecipe.IngridietsString == null)
                {
                    throw new Exception();
                }

                var recipeToUpdate = db.Recipes.Find(editedRecipe.Id);

                string imagePath = "";
                string imageToDelete = "";
                if (editedRecipe.ImageFile != null)
                {
                    imagePath = UploadFile(editedRecipe.ImageFile, "images/thumbnails");
                    imageToDelete = recipeToUpdate.ImageUrl;
                }


                recipeToUpdate.Name = editedRecipe.Name;
                recipeToUpdate.Text = editedRecipe.Text;
                recipeToUpdate.Tags = editedRecipe.IngridietsString;
                if (imagePath != "")
                {
                    recipeToUpdate.ImageUrl = imagePath;
                }

                db.Entry(recipeToUpdate).State = EntityState.Modified;
                db.SaveChanges();

                if (imageToDelete != "")
                {
                    System.IO.File.Delete(HttpContext.Server.MapPath("~/Content/images/thumbnails/" + imageToDelete));
                }

                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: Recipe/Delete/5
        public ActionResult Delete(int id)
        {
            var recipeToDelite = db.Recipes.Find(id);
            if (recipeToDelite == null)
            {
                return HttpNotFound();
            }
            if (recipeToDelite.ImageUrl != null)
            {
                try
                {
                    System.IO.File.Delete(HttpContext.Server.MapPath("~/Content/images/thumbnails/" + recipeToDelite.ImageUrl));
                }
                catch { }
            }

            db.Recipes.Remove(recipeToDelite);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            try
            {
                var commentToAdd = (Comment)comment;
                commentToAdd.UserId = User.Identity.GetUserId();
                db.Comments.Add(commentToAdd);
                db.SaveChanges();
                return RedirectToAction("Details", "Recipe", new { id = comment.RecipeId });
            }
            catch
            {
                return RedirectToAction("Details", "Recipe", new { id = comment.RecipeId });
            }
        }

        [HttpGet]
        public ActionResult DeleteComment(int id)
        {
            try
            {
                var commentToDelete = db.Comments.Find(id);
                if (commentToDelete == null)
                {
                    throw new Exception();
                }

                db.Comments.Remove(commentToDelete);
                db.SaveChanges();
                return RedirectToAction("Details", "Recipe", new { id = commentToDelete.RecipeId });
            }
            catch
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Like(LikeModel like)
        {
            var l = db.Likes.FirstOrDefault(i => i.UserId == like.UserId && i.RecipeId == like.RecipeId);
            if (l != null)
            {
                if (l.Value == 0)
                    l.Value = 1;
                else
                    l.Value = 0;

                var val = l.Value;

                db.Entry(l).State = EntityState.Modified;
                await db.SaveChangesAsync();


                return RedirectToAction("Details", "Recipe", new { id = like.RecipeId });
            }
            else
            {
                var likeToAdd = new Like()
                {
                    UserId = like.UserId,
                    RecipeId = like.RecipeId,
                    Value = 1
                };
                db.Likes.Add(likeToAdd);

                await db.SaveChangesAsync();

                return RedirectToAction("Details", "Recipe", new { id = like.RecipeId });
            }
        }
    }
}