using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using App472.WebUI.Models;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace App472.WebUI
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    public class OwinIdentityConfig
    {
        // See
        // https://go.microsoft.com/fwlink/?LinkId=301864
        public void Configuration(IAppBuilder app)
        {
            // See
            // https://stackoverflow.com/questions/31960433/adding-asp-net-mvc5-identity-authentication-to-an-existing-project
            app.CreatePerOwinContext(() => new IDDBContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);
            app.CreatePerOwinContext<RoleManager<AppRole>>((options, context) => new RoleManager<AppRole>(new RoleStore<AppRole>(context.Get<IDDBContext>())));
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}





