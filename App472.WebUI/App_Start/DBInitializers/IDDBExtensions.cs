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

namespace App472.WebUI.App_Start
{

    public static class IDDBExtensions
    {
        private static string secretAdminPWHashed;
        public static IList<AppUser> Users; // static variable used to seed the database

        // Static method to extend our context class,
        // so we can call some common seed operations on it,
        // no matter if we are in debug or release modes.
        public static void SeedDomainObjects(this IDDBContext context)
        {
            // Executes second
            // Seed the Domain objects...

            // Populate products
            IList<InStockProduct> products = new List<InStockProduct>();
            ProductsWater.Get(ref products);
            ProductsSoccer.Get(ref products);
            ProductsChess.Get(ref products);
            context.Products.AddRange(products);

            // Populate orders
            IList<Order> orders = new List<Order>();
            Int32 orderIdStart = 1;  // MSSQL auto increment starts at 1 for orderId
            Orders111.AddToContext(ref orders, ref products, ref context, ref orderIdStart);
            Orders112.AddToContext(ref orders, ref products, ref context, ref orderIdStart);

            IList<Guest> guests = new List<Guest>();
            Guid guestID;
            guestID = Guid.NewGuid();
            guests.Add(new Guest
            {
                Id = guestID,
                FirstName = "Dye",
                LastName = "McDonald",
                Email = "guest-113@gmail.com",
                Orders = Orders113.GetOrders(ref products, ref context, ref orderIdStart, guestID)
            });
            guestID = Guid.NewGuid();
            guests.Add(new Guest
            {
                Id = guestID,
                FirstName = "Lamond",
                LastName = "Forester",
                Email = "guest-114@gmail.com",
                Orders = Orders114.GetOrders(ref products, ref context, ref orderIdStart, guestID)
            });
            context.Guests.AddRange(guests);
        }

        // Static method to extend our context class,
        // so we can call some common seed operations on it,
        // no matter if we are in debug or release modes.
        public static void SeedIDContext(this IDDBContext context)
        {
            // Executes first
            // Seed the Identity tables...

            IPasswordHasher hasher = new PasswordHasher();
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
            SeedAppUser("111");
            SeedAppUser("112");
            // 113 is Guest
            // 114 is Guest
            //SeedAppUser("115");
            //SeedAppUser("116");
            //SeedAppUser("117");
            //SeedAppUser("118");
            context.Users.AddOrUpdate(Users.ToArray());
        }

        private static void SeedAppUser(string userID)
        {
            AppUser user = new AppUser
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
            Users.Add(user);
        }

        private static string UserKey(string id, string suffix)
        {
            return "users:" + id + ":" + suffix;
        }

        private static DateTime? GetLockoutUtcDaysFromNow(string days)
        {
            return string.IsNullOrEmpty(days) ? (DateTime?)null : DateTime.UtcNow.AddDays(Double.Parse(days));
        }
    }
}