using App472.WebUI.Domain.Abstract;
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
        }

        // User Accounts
        public ViewResult Index(string returnUrl)
        {
            string url = GetTabReturnUrl(returnUrl);
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers.ToList();
            return View(new AdminUserAccViewModel{
                CurrentPageNavText = AppNavs.UsersNavText,
                Guests = guestRepo.Guests,
                FullUsers = fullUsers
            });
        }

        [HttpPost]
        public JsonResult LockedOutUpdate(LockedOutUpdateDTO model){
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            LockoutUpdateResultDTO result = fullUserRepo.LockedOutUpdate(model);
            return Json(new { LockoutEndDateUtc = result.Utc, Attempts = result.Attempts }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateUsername(UsernameUpdateDTO model)
        {
            if (!ModelState.IsValid){
                var errors = MyExtensions.ModelErrors(ModelState);
                return Json(new { success = false, errorMessage = "failed validation", errors = errors }, JsonRequestBehavior.AllowGet);
            }
            bool successBool = false;
            if (model.GuestID != null){
                // Guest
                return Json(new { success = false, errorMessage = "Guest Username cannot be changed. Please register a User account."}, JsonRequestBehavior.AllowGet);
            }
            else{
                // User
                fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                successBool = fullUserRepo.UsernameUpdate(model);
            }
            return Json(new { success = successBool, username = model.Username }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateEmail(EmailUpdateDTO model)
        {
            if (!ModelState.IsValid){
                var errors = MyExtensions.ModelErrors(ModelState);
                return Json(new { success = false, errorMessage = "failed validation", errors = errors }, JsonRequestBehavior.AllowGet);
            }
            bool successBool = false;
            if (model.GuestID != null){
                // Guest
                successBool = guestRepo.EmailUpdate(model);
            }else{
                // User
                fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                successBool = fullUserRepo.EmailUpdate(model);
            }
            return Json(new { success = successBool, email = model.Email }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdatePhone(PhoneUpdateDTO model)
        {
            if (!ModelState.IsValid)
            {
                var errors = MyExtensions.ModelErrors(ModelState);
                return Json(new { success = false, errorMessage = "failed validation", errors = errors }, JsonRequestBehavior.AllowGet);
            }
            bool successBool = false;
            if (model.GuestID != null)
            {
                // Guest
                return Json(new { success = false, errorMessage = "Guest Phone Number cannot be changed. Please register a User account." }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                // User
                fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                successBool = fullUserRepo.PhoneUpdate(model);
            }
            return Json(new { success = successBool, phone = model.Phone }, JsonRequestBehavior.AllowGet);
        }
    }
}