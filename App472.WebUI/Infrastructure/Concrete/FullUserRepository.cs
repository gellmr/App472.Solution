﻿using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
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
                    Int32 appUserId = Int32.Parse(appUser.Id);
                    fullUsers.Add(
                        new FullUser{
                            AppUser = appUser,
                            Orders = ordersRepo.Orders.Where(o => o.UserID == appUserId)
                    });
                }
                return fullUsers;
            }
        }

        public void LockedOutUpdate(LockedOutUpdateDTO updateModel)
        {
            AppUser user = AppUserManager.Users.FirstOrDefault(u => u.Id == updateModel.UserID.ToString()); // get user
            user.LockoutEnabled = updateModel.Lock; // apply lock / unlock
            if (updateModel.Lock == true){
                user.LockoutEndDateUtc = DateTime.UtcNow.AddSeconds(60); // lock for 60 seconds
            }
            AppUserManager.Update(user); // update database
        }
    }
}