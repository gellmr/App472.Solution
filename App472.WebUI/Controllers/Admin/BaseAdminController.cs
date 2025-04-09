using App472.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App472.WebUI.Controllers.Admin
{
    public class BaseAdminController : BaseController
    {
        public BaseAdminController()
        {
            if (AdminSessUser == null)
            {
                // Initiate the admin session user object
                System.Web.HttpContext.Current.Session.Add(SessExtensions.AdminSessUserKeyName, new AdminSessUser());
            }
        }

        public AdminSessUser AdminSessUser
        {
            get
            {
                return System.Web.HttpContext.Current.GetAdminSessUser();
            }
        }
    }
}