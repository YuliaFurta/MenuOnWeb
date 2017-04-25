namespace MenuOnWebFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLogin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "Login", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "Login");
        }
    }
}
