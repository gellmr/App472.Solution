namespace App472.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDomainEntities : DbMigration
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
                        GuestID = c.Guid(),
                        UserID = c.Int(),
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
                .ForeignKey("dbo.Guests", t => t.GuestID)
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
                .ForeignKey("dbo.Products", t => t.Product_ProductID)
                .Index(t => t.Order_OrderID)
                .Index(t => t.Product_ProductID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Category = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "GuestID", "dbo.Guests");
            DropForeignKey("dbo.OrderedProducts", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderedProducts", "Order_OrderID", "dbo.Orders");
            DropIndex("dbo.OrderedProducts", new[] { "Product_ProductID" });
            DropIndex("dbo.OrderedProducts", new[] { "Order_OrderID" });
            DropIndex("dbo.Orders", new[] { "GuestID" });
            DropTable("dbo.Products");
            DropTable("dbo.OrderedProducts");
            DropTable("dbo.Orders");
            DropTable("dbo.Guests");
        }
    }
}
