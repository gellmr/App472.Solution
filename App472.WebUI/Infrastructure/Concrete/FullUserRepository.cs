using App472.Domain.Abstract;
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

namespace App472.WebUI.Infrastructure.Concrete
{
    // The AppUser objects are stored in a default .NET Identity database
    // The EF objects are stored in another database
    // This repository can access the AppUserManager, AppUser objects, and EF objects.
    // The FullUser class encapsulates an AppUser object, with its EF objects.
    public class FullUserRepository : IFullUserRepository
    {
        private IOrdersRepository ordersRepo { get; set; }

        public FullUserRepository(IOrdersRepository or){
            ordersRepo = or;
        }

        public IEnumerable<FullUser> FullUsers(AppUserManager appUserManager)
        {
            // Return the encapsulated AppUsers, with their EF objects.
            List<FullUser> fullUsers = new List<FullUser>();
            foreach (AppUser appUser in appUserManager.Users)
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
}