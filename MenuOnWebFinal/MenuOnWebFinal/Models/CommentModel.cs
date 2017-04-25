
namespace MenuOnWebFinal.Models
{
    using System;

    public class CommentModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int RecipeId { get; set; }

        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public string AuthorLogin { get; set; }

        public string AuthorAvatar { get; set; }

        public static explicit operator CommentModel(Comment commentEntity)
        {
            var comment = new CommentModel()
            {
                Id = commentEntity.Id,
                UserId = commentEntity.UserId,
                RecipeId = commentEntity.RecipeId,
                Text = commentEntity.Text,
                CreateDate = commentEntity.CreateDate,
               // AuthorLogin = commentEntity.User.Login,
               // AuthorAvatar = commentEntity.User.AvatarUrl
            };

            return comment;
        }

        public static explicit operator Comment(CommentModel commentModel)
        {
            var comment = new Comment()
            {
                UserId = commentModel.UserId,
                RecipeId = commentModel.RecipeId,
                Text = commentModel.Text,
                CreateDate = commentModel.CreateDate,
            };

            return comment;
        }
    }
}