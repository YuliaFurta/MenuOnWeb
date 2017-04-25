namespace MenuOnWebFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablesForRecipeDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        RecipeId = c.Int(nullable: false),
                        Text = c.String(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipe", t => t.RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RecipeId);
            
            CreateTable(
                "dbo.Recipe",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 30),
                        Text = c.String(nullable: false),
                        ImageUrl = c.String(),
                        Tags = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Likes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RecipeId = c.Int(nullable: false),
                        Value = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RecipeId })
                .ForeignKey("dbo.Recipe", t => t.RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RecipeId);
            
            AddColumn("dbo.User", "Recipe_Id", c => c.Int());
            CreateIndex("dbo.User", "Recipe_Id");
            AddForeignKey("dbo.User", "Recipe_Id", "dbo.Recipe", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comment", "UserId", "dbo.User");
            DropForeignKey("dbo.User", "Recipe_Id", "dbo.Recipe");
            DropForeignKey("dbo.Recipe", "UserId", "dbo.User");
            DropForeignKey("dbo.Likes", "UserId", "dbo.User");
            DropForeignKey("dbo.Likes", "RecipeId", "dbo.Recipe");
            DropForeignKey("dbo.Comment", "RecipeId", "dbo.Recipe");
            DropIndex("dbo.User", new[] { "Recipe_Id" });
            DropIndex("dbo.Likes", new[] { "RecipeId" });
            DropIndex("dbo.Likes", new[] { "UserId" });
            DropIndex("dbo.Recipe", new[] { "UserId" });
            DropIndex("dbo.Comment", new[] { "RecipeId" });
            DropIndex("dbo.Comment", new[] { "UserId" });
            DropColumn("dbo.User", "Recipe_Id");
            DropTable("dbo.Likes");
            DropTable("dbo.Recipe");
            DropTable("dbo.Comment");
        }
    }
}
