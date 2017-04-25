using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MenuOnWebFinal.Models
{
    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int RecipeId { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual User User { get; set; }
    }
}