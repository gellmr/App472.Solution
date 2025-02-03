using App472.WebUI.Infrastructure.Abstract;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

using System.Configuration;
using App472.WebUI.Infrastructure.DTO;
using Microsoft.AspNet.Identity;
using App472.WebUI.Domain.Abstract;

namespace App472.WebUI.Infrastructure.Concrete
{
    // The AppUser objects are stored in a default .NET Identity database
    // The EF objects are stored in another database
    // This repository can access the AppUserManager, AppUser objects, and EF objects.
    // The FullUser class encapsulates an AppUser object, with its EF objects.
    public class FullUserRepository : IFullUserRepository
    {
        private IOrdersRepository ordersRepo { get; set; }
        public AppUserManager AppUserManager { get; set; }

        public FullUserRepository(IOrdersRepository or){
            ordersRepo = or;
        }

        public IEnumerable<FullUser> FullUsers {
            get {
                // Return the encapsulated AppUsers, with their EF objects.
                List<FullUser> fullUsers = new List<FullUser>();
                foreach (AppUser appUser in AppUserManager.Users)
                {
                    string appUserId = appUser.Id;
                    fullUsers.Add(
                        new FullUser{
                            AppUser = appUser,
                            Orders = ordersRepo.Orders.Where(o => o.UserID == appUserId)
                    });
                }
                return fullUsers;
            }
        }

        public LockoutUpdateResultDTO LockedOutUpdate(LockedOutUpdateDTO updateModel)
        {
            AppUser user = AppUserManager.Users.FirstOrDefault(u => u.Id == updateModel.UserID.ToString()); // get user
            user.LockoutEnabled = updateModel.Lock; // apply lock / unlock
            if (updateModel.Lock == true){
                // Admin had decided to lock this user.
                // Lockout date is stored as nullable UTC datetime. So if its 11pm (UTC+08:00) in Perth right now then we store 3pm which is the UTC value.
                user.LockoutEndDateUtc = DateTime.UtcNow.AddMinutes(5); // lock for 5 minutes
            }else{
                // Admin has decided to unlock this user.
                user.LockoutEndDateUtc = null; // clear the lockout end date.
                user.AccessFailedCount = 0; // clear the attempts to start over
            }
            AppUserManager.Update(user); // update database
            return new LockoutUpdateResultDTO { Utc = user.LockoutEndDateUtc, Attempts = user.AccessFailedCount };
        }

        public bool UsernameUpdate(UsernameUpdateDTO updateModel)
        {
            AppUser user = AppUserManager.Users.FirstOrDefault(u => u.Id == updateModel.UserID.ToString()); // get user
            user.UserName = updateModel.Username;
            AppUserManager.Update(user); // update database
            return true;
        }

        public bool EmailUpdate(EmailUpdateDTO updateModel)
        {
            AppUser user = AppUserManager.Users.FirstOrDefault(u => u.Id == updateModel.UserID.ToString()); // get user
            user.Email = updateModel.Email;
            AppUserManager.Update(user); // update database
            return true;
        }
    }
}