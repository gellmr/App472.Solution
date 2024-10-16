﻿using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.WebUI.Models;
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
        public ViewResult Index()
        {
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
           
            return View(new AdminUserViewModel{
                LinkText = "Edit Users",
                Users = userManager.Users
            });
        }
    }
}