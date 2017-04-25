
namespace MenuOnWebFinal.Models
{
    using System.Collections.Generic;

    public class UserModel
    {
        public string Id { get; set; }
        public string Login { get; set; }
        public string AvatarUrl { get; set; }
        public List<ViewRecipe> MyRecipes { get; set; }
        public List<ViewRecipe> FavouriteRecipes { get; set; }

        public static explicit operator UserModel(User userEntity)
        {
            var myRecipesView = new List<ViewRecipe>();
            foreach (var item in userEntity.Recipes)
            {
                myRecipesView.Add((ViewRecipe)item);
            }
            var myFavRecipesView = new List<ViewRecipe>();
            foreach (var item in userEntity.FavouriteRecipes)
            {
                myFavRecipesView.Add((ViewRecipe)item);
            }

            var user = new UserModel()
            {
                Id = userEntity.Id,
                Login = userEntity.UserName,
                AvatarUrl = userEntity.AvatarUrl,
                MyRecipes = myRecipesView,
                FavouriteRecipes = myFavRecipesView
            };

            return user;
        }
    }
}