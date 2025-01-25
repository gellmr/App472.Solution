namespace App472.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderHasUserIdString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "UserID", c => c.String(maxLength: 128));
            CreateIndex("dbo.Orders", "UserID");
            AddForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "UserID" });
            AlterColumn("dbo.Orders", "UserID", c => c.Int());
        }
    }
}
