using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure.Abstract;
using App472.WebUI.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

using System.Configuration;

namespace App472.WebUI.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(){}
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                var authManager = HttpContext.GetOwinContext().Authentication;
                AppUser user = userManager.Find(model.UserName, model.Password);
                if(user != null)
                {
                    var ident = userManager.CreateIdentity(user,DefaultAuthenticationTypes.ApplicationCookie);
                    //use the instance that has been created. 
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Logout(){
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            TempData["message"] = "Successfully logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}