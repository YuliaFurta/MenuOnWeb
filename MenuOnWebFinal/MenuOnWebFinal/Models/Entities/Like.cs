using System;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MenuOnWebFinal.Models
{
    public partial class Like
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RecipeId { get; set; }

        public int Value { get; set; }

        public virtual Recipe Recipe { get; set; }

        public virtual User User { get; set; }
    }
}