namespace App472.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderPayments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderPayments",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                        Amount = c.Decimal(precision: 18, scale: 2),
                        Date = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .Index(t => t.OrderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderPayments", "OrderID", "dbo.Orders");
            DropIndex("dbo.OrderPayments", new[] { "OrderID" });
            DropTable("dbo.OrderPayments");
        }
    }
}
