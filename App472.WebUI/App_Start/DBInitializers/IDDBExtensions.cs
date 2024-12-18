﻿using App472.Domain.Concrete;
using App472.Domain.Entities;
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

namespace App472.WebUI.App_Start
{
    public static class IDDBExtensions
    {
        private static string secretAdminPWHashed;

        // Static method to extend our context class,
        // so we can call some common seed operations on it,
        // no matter if we are in debug or release modes.
        public static void SeedIDContext(this IDDBContext context)
        {
            // perform seed operations...

            IPasswordHasher hasher = new PasswordHasher();
            IList<AppUser> users = new List<AppUser>();
            string secretAdminPassword = ConfigurationManager.AppSettings["SecretAdminPassword"];
            if (secretAdminPassword == null)
            {
                throw new Exception("Secrets not found");
            }
            secretAdminPWHashed = hasher.HashPassword(secretAdminPassword);

            // see https://github.com/aspnet/MicrosoftConfigurationBuilders/blob/main/samples/SampleWebApp/App_Data/settings.json
            // see https://github.com/aspnet/MicrosoftConfigurationBuilders/blob/main/samples/SampleWebApp/Web.config

            // populate users
            GetValues("111", ref users);
            GetValues("112", ref users);

            context.Users.AddOrUpdate(users.ToArray());
        }

        private static void GetValues(string userID, ref IList<AppUser> users)
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
            users.Add(user);
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