using App472.Domain.Abstract;
using App472.Domain.Concrete;
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
        private IGuestRepository guestRepo;

        public AdminUserController(IGuestRepository gRepo){
            guestRepo = gRepo;
        }

        public ViewResult Index(){
            var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IEnumerable<Guest> guests = guestRepo.Guests;
            return View(new AdminUserViewModel{
                LinkText = "Edit Users",
                Users = userManager.Users,
                Guests = guests
            });
        }
    }
}