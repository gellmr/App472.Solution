using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace App472.WebUI
{
    public partial class Startup
    {
        // See
        // https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // See IdentityConfig.cs ...Configuration(IAppBuilder app)
        }
    }
}