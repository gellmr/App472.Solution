using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using Microsoft.Configuration.ConfigurationBuilders;
using System.Configuration;
using System.Security;
using System.Web.Helpers;
using Microsoft.Ajax.Utilities;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Infrastructure;

namespace App472.WebUI.App_Start
{
    public static class IDDBExtensions
    {
        private static string secretAdminPWHashed;

        // static variables used to seed the database
        public static IDictionary<string,Guest> Guests;
        public static IList<AppUser> Users;
        public static IList<Order> Orders;
        public static IList<OrderedProduct> OrderedProducts;
        public static IList<OrderPayment> OrderPayments;

        // Static method to extend our context class,
        // so we can call some common seed operations on it,
        // no matter if we are in debug or release modes.
        public static void SeedIDContext(this IDDBContext context)
        {
            // Executes first
            // Seed the Identity tables...

            IPasswordHasher hasher = new PasswordHasher();
            Guests = new Dictionary<string,Guest>();
            Users = new List<AppUser>();
            string secretAdminPassword = ConfigurationManager.AppSettings["SecretAdminPassword"];
            if (secretAdminPassword == null)
            {
                throw new Exception("Secrets not found");
            }
            secretAdminPWHashed = hasher.HashPassword(secretAdminPassword);

            // see https://github.com/aspnet/MicrosoftConfigurationBuilders/blob/main/samples/SampleWebApp/App_Data/settings.json
            // see https://github.com/aspnet/MicrosoftConfigurationBuilders/blob/main/samples/SampleWebApp/Web.config

            // populate users
            //                      Index   Comment
            SeedAppUser("111");  // 0       Admin Login
            SeedAppUser("112");  // 1       GUEST
            SeedAppUser("113");  // 2       GUEST
            for (int u = 114; u <= 150; u++)
            {
                SeedAppUser(u.ToString());  // 3..39  AppUsers
            }
            context.Users.AddOrUpdate(Users.ToArray());

            // Populate orders
            Orders = new List<Order>();
            for(int ord = 1; ord <= 70; ord++)
            {
                SeedOrder(ord.ToString());
            }
            context.Orders.AddOrUpdate(Orders.ToArray());

            // Populate ordered products
            OrderedProducts = new List<OrderedProduct>(); // listed as an array in the json, so we have to use zero for the first index.
            int opCount = 200;
            for (int idx = 0; idx < opCount; idx++)
            {
                SeedOrderedProduct(idx.ToString());
            }
            context.OrderedProducts.AddOrUpdate(OrderedProducts.ToArray());

            // Populate products
            IList<InStockProduct> products = new List<InStockProduct>();
            ProductsWater.Get(ref products);
            ProductsSoccer.Get(ref products);
            ProductsChess.Get(ref products);
            context.InStockProducts.AddRange(products);
        }

        /*
        // Load our seed.json file containing Users
        {
          "appSettings": {
            "users": {
              "999": {
                "IsGuest": false,
                "Id": "****",
                "Email": "*************",
                "EmailConfirmed": true,
                ...
                "SecurityStamp": "Guid.NewGuid().ToString()",
                "PhoneNumber": "** **** ****",
                "PhoneNumberConfirmed": true,
                "TwoFactorEnabled": false,
                "LockoutEndDateUtc": null,
                "LockoutEnabled": false,
                "AccessFailedCount": 0,
                "UserName": "*******"
              },
              // ... more users
            },
            "orders": {...}
          }
        }
        */
        private static void SeedAppUser(string userID)
        {
            bool isGuest = Boolean.Parse( ConfigurationManager.AppSettings[UserKey(userID, "IsGuest")]);

            //AppUser user = new AppUser
            var aUser = new
            {
                Id = ConfigurationManager.AppSettings[UserKey(userID, "Id")],
                Email = ConfigurationManager.AppSettings[UserKey(userID, "Email")],
                EmailConfirmed = Boolean.Parse(ConfigurationManager.AppSettings[UserKey(userID, "EmailConfirmed")]),
                PasswordHash = secretAdminPWHashed,
                SecurityStamp = Guid.NewGuid().ToString(),
                PhoneNumber = ConfigurationManager.AppSettings[UserKey(userID, "PhoneNumber")],
                PhoneNumberConfirmed = Boolean.Parse(ConfigurationManager.AppSettings[UserKey(userID, "PhoneNumberConfirmed")]),
                TwoFactorEnabled = Boolean.Parse(ConfigurationManager.AppSettings[UserKey(userID, "TwoFactorEnabled")]),
                LockoutEndDateUtc = GetLockoutUtcDaysFromNow(ConfigurationManager.AppSettings[UserKey(userID, "LockoutEndDateUtc")]),
                LockoutEnabled = Boolean.Parse(ConfigurationManager.AppSettings[UserKey(userID, "LockoutEnabled")]),
                AccessFailedCount = Int32.Parse(ConfigurationManager.AppSettings[UserKey(userID, "AccessFailedCount")]),
                UserName = ConfigurationManager.AppSettings[UserKey(userID, "UserName")]
            };
            if(isGuest)
            {
              // Guest
              string[] names = aUser.UserName.Split(' ');
              Guests.Add(userID, new Guest{
                  ID = MyExtensions.ToNullableGuid( aUser.Id ),
                  Email = aUser.Email,
                  FirstName = names[0],
                  LastName = names[1]
              });
            }else{
              // AppUser
              Users.Add(new AppUser{
                Id = aUser.Id,
                Email = aUser.Email,
                EmailConfirmed = aUser.EmailConfirmed,
                PasswordHash = aUser.PasswordHash,
                SecurityStamp = aUser.SecurityStamp,
                PhoneNumber = aUser.PhoneNumber,
                PhoneNumberConfirmed = aUser.PhoneNumberConfirmed,
                TwoFactorEnabled = aUser.TwoFactorEnabled,
                LockoutEndDateUtc = aUser.LockoutEndDateUtc,
                LockoutEnabled = aUser.LockoutEnabled,
                AccessFailedCount = aUser.AccessFailedCount,
                UserName = aUser.UserName
              });
            };
        }

        /*
        // Load our seed.json file containing Orders
        {
          "appSettings": {
            "users": {...},
            "orders": {
              "1": {
                "UserID": "999",   <--- Can have many orders belonging to one user.
                "ID": "1",
                "OrderPlacedDate": "2025-05-27 13:12:00.0000000 +08:00",
                "PaymentReceivedDate": "2025-05-27 13:22:00.0000000 +08:00",
                "ReadyToShipDate": "2025-05-27 13:22:00.0000000 +08:00",
                "ShipDate": "null",
                "ReceivedDate": "null",
                "BillingAddress": "NNN Street Name, Suburb WA POSTC",
                "ShippingAddress": "NNN Street Name, Suburb WA POSTC",
                "OrderStatus": "ReadyToShip"
              },
              // ... more orders
            }
        }
        */
        private static void SeedOrder(string orderID)
        {
            string userId = ConfigurationManager.AppSettings[OrderKey(orderID, "UserID")];
            AppUser user = Users.FirstOrDefault( u => u.Id.Equals(userId) );
            Guest guest; Guests.TryGetValue(userId, out guest); // FirstOrDefault( g => g.ID.Equals(userId) );
            Order ord = new Order
            {
                ID = (Nullable<Int32>) Int32.Parse(         ConfigurationManager.AppSettings[OrderKey(orderID, "ID")]),
                OrderPlacedDate = GetOrderDateTime(         ConfigurationManager.AppSettings[OrderKey(orderID, "OrderPlacedDate")] ),
                PaymentReceivedDate = GetOrderDateTime(     ConfigurationManager.AppSettings[OrderKey(orderID, "PaymentReceivedDate")] ),
                ReadyToShipDate = GetOrderDateTime(         ConfigurationManager.AppSettings[OrderKey(orderID, "ReadyToShipDate")] ),
                ShipDate = GetOrderDateTime(                ConfigurationManager.AppSettings[OrderKey(orderID, "ReadyToShipDate")] ),
                ReceivedDate = GetOrderDateTime(            ConfigurationManager.AppSettings[OrderKey(orderID, "ReceivedDate")] ),
                BillingAddress =                            ConfigurationManager.AppSettings[OrderKey(orderID, "BillingAddress")],
                ShippingAddress =                           ConfigurationManager.AppSettings[OrderKey(orderID, "ShippingAddress")],
                OrderStatus =                               ConfigurationManager.AppSettings[OrderKey(orderID, "OrderStatus")],
            };
            if (guest != null){
                ord.Guest = guest;
                ord.GuestID = guest.ID;
            }
            else{
                ord.AppUser = user;
                ord.UserID = userId;
            }
            Orders.Add(ord);
        }

        private static string UserKey(string id, string suffix){
            return "users:" + id + ":" + suffix;
        }

        private static string OrderKey(string id, string suffix){
            return "orders:" + id + ":" + suffix;
        }

        private static string OrderedProductKey(string id, string suffix){
            return "orderedproducts:" + id + ":" + suffix;
        }

        private static Nullable<DateTimeOffset> GetOrderDateTime(string input){
            try{
                return (Nullable<DateTimeOffset>) DateTimeOffset.Parse( input );
            }
            catch(FormatException e){
                return null;
            }
        }

        /*
        // Load our seed.json file containing Ordered Products
        {
          "appSettings": {
            "users": {...},
            "orders": {...},
            "orderedproducts":
            [
              {
                "ID": "1",
                "OrderID": "1",   <------- An Order can have many OrderedProducts
                "InStockProductID": "8",
                "Quantity": "1"
              },
              // ...more ordered products
            ]
        }
        */
        private static void SeedOrderedProduct( string idx )
        {
            // The json holds an array of ordered products where the first index is zero. So idx == 0 for our first item.
            OrderedProduct op = new OrderedProduct
            {
                ID = (Nullable<Int32>) Int32.Parse(             ConfigurationManager.AppSettings[OrderedProductKey(idx, "ID")]),
                OrderID = (Nullable<Int32>)Int32.Parse(         ConfigurationManager.AppSettings[OrderedProductKey(idx, "OrderID")]),
                InStockProductID = Int32.Parse(                 ConfigurationManager.AppSettings[OrderedProductKey(idx, "InStockProductID")]),
                Quantity = Int32.Parse(                         ConfigurationManager.AppSettings[OrderedProductKey(idx, "Quantity")]),
            };

            OrderedProducts.Add(op);
        }

        private static DateTime? GetLockoutUtcDaysFromNow(string days)
        {
            return string.IsNullOrEmpty(days) ? (DateTime?)null : DateTime.UtcNow.AddDays(Double.Parse(days));
        }
    }
}