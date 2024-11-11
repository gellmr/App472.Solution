using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminUserAccController : BaseController
    {
        // User Accounts Controller

        private IGuestRepository guestRepo;
        private App472.WebUI.Infrastructure.Abstract.IFullUserRepository fullUserRepo;

        public AdminUserAccController(IGuestRepository gRepo, App472.WebUI.Infrastructure.Abstract.IFullUserRepository fRepo){
            guestRepo = gRepo;
            fullUserRepo = fRepo;
        }

        // User Accounts
        public ViewResult Index(string returnUrl)
        {
            string url = GetTabReturnUrl(returnUrl);
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers(userManager).ToList();
            return View(new AdminUserAccViewModel{
                CurrentPageNavText = AppNavs.UsersNavText,
                ReturnUrl = url,
                Guests = guestRepo.Guests,
                FullUsers = fullUsers
            });
        }
    }
}