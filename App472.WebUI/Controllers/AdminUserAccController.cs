using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Infrastructure.DTO;
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
            //fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); // cant do this in constructor for some reason
        }

        // User Accounts
        public ViewResult Index(string returnUrl)
        {
            string url = GetTabReturnUrl(returnUrl);
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers.ToList();
            return View(new AdminUserAccViewModel{
                CurrentPageNavText = AppNavs.UsersNavText,
                ReturnUrl = url,
                Guests = guestRepo.Guests,
                FullUsers = fullUsers
            });
        }

        [HttpPost]
        public JsonResult LockedOutUpdate(LockedOutUpdateDTO model){
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            fullUserRepo.LockedOutUpdate(model);
            return Json(new { }, JsonRequestBehavior.AllowGet); // need to return datetime of the lockout
        }
    }
}