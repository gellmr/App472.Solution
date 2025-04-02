namespace App472.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DomainEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        ID = c.Guid(nullable: false),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserID = c.String(maxLength: 128),
                        GuestID = c.Guid(),
                        OrderPlacedDate = c.DateTimeOffset(precision: 7),
                        PaymentReceivedDate = c.DateTimeOffset(precision: 7),
                        ReadyToShipDate = c.DateTimeOffset(precision: 7),
                        ShipDate = c.DateTimeOffset(precision: 7),
                        ReceivedDate = c.DateTimeOffset(precision: 7),
                        BillingAddress = c.String(),
                        ShippingAddress = c.String(),
                        OrderStatus = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.Guests", t => t.GuestID)
                .Index(t => t.UserID)
                .Index(t => t.GuestID);
            
            CreateTable(
                "dbo.OrderedProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(),
                        InStockProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.InStockProducts", t => t.InStockProductID, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.OrderID)
                .Index(t => t.OrderID)
                .Index(t => t.InStockProductID);
            
            CreateTable(
                "dbo.InStockProducts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
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
            DropForeignKey("dbo.OrderedProducts", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderedProducts", "InStockProductID", "dbo.InStockProducts");
            DropForeignKey("dbo.Orders", "GuestID", "dbo.Guests");
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.OrderPayments", new[] { "OrderID" });
            DropIndex("dbo.OrderedProducts", new[] { "InStockProductID" });
            DropIndex("dbo.OrderedProducts", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "GuestID" });
            DropIndex("dbo.Orders", new[] { "UserID" });
            DropTable("dbo.OrderPayments");
            DropTable("dbo.InStockProducts");
            DropTable("dbo.OrderedProducts");
            DropTable("dbo.Orders");
            DropTable("dbo.Guests");
        }
    }
}
