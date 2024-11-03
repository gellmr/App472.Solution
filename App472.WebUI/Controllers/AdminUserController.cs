﻿using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminUserController : BaseController
    {
        private IGuestRepository guestRepo;
        private App472.WebUI.Infrastructure.Abstract.IFullUserRepository fullUserRepo;

        public AdminUserController(IGuestRepository gRepo, App472.WebUI.Infrastructure.Abstract.IFullUserRepository fRepo){
            guestRepo = gRepo;
            fullUserRepo = fRepo;
        }

        public ViewResult Index()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers(userManager).ToList();
            return View(new AdminUserViewModel{
                LinkText = "User Orders",
                Guests = guestRepo.Guests,
                Users = fullUsers
            });
        }
    }
}