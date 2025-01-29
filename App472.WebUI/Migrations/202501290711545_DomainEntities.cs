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
                        Id = c.Guid(nullable: false),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
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
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserID)
                .ForeignKey("dbo.Guests", t => t.GuestID)
                .Index(t => t.UserID)
                .Index(t => t.GuestID);
            
            CreateTable(
                "dbo.OrderedProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Order_OrderID = c.Int(),
                        Product_ProductID = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.Order_OrderID)
                .ForeignKey("dbo.InStockProducts", t => t.Product_ProductID)
                .Index(t => t.Order_OrderID)
                .Index(t => t.Product_ProductID);
            
            CreateTable(
                "dbo.InStockProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID);
            
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
            DropForeignKey("dbo.OrderedProducts", "Product_ProductID", "dbo.InStockProducts");
            DropForeignKey("dbo.OrderedProducts", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.Orders", "GuestID", "dbo.Guests");
            DropForeignKey("dbo.Orders", "UserID", "dbo.AspNetUsers");
            DropIndex("dbo.OrderPayments", new[] { "OrderID" });
            DropIndex("dbo.OrderedProducts", new[] { "Product_ProductID" });
            DropIndex("dbo.OrderedProducts", new[] { "Order_OrderID" });
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
