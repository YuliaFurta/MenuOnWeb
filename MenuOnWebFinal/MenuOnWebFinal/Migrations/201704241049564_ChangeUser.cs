namespace MenuOnWebFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.User", "Recipe_Id", "dbo.Recipe");
            DropIndex("dbo.Recipe", new[] { "UserId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.User", new[] { "Recipe_Id" });
            CreateTable(
                "dbo.Favourites",
                c => new
                    {
                        RecipeId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.RecipeId, t.UserId })
                .ForeignKey("dbo.Recipe", t => t.RecipeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.RecipeId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.User", "AvatarUrl", c => c.String());
            AlterColumn("dbo.Recipe", "UserId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 256, unicode: false));
            CreateIndex("dbo.Recipe", "UserId");
            CreateIndex("dbo.User", "UserName", unique: true, name: "UserNameIndex");
            DropColumn("dbo.User", "Recipe_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.User", "Recipe_Id", c => c.Int());
            DropForeignKey("dbo.Favourites", "UserId", "dbo.User");
            DropForeignKey("dbo.Favourites", "RecipeId", "dbo.Recipe");
            DropIndex("dbo.Favourites", new[] { "UserId" });
            DropIndex("dbo.Favourites", new[] { "RecipeId" });
            DropIndex("dbo.User", "UserNameIndex");
            DropIndex("dbo.Recipe", new[] { "UserId" });
            AlterColumn("dbo.User", "UserName", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.Recipe", "UserId", c => c.String(maxLength: 128));
            DropColumn("dbo.User", "AvatarUrl");
            DropTable("dbo.Favourites");
            CreateIndex("dbo.User", "Recipe_Id");
            CreateIndex("dbo.User", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.Recipe", "UserId");
            AddForeignKey("dbo.User", "Recipe_Id", "dbo.Recipe", "Id");
        }
    }
}
