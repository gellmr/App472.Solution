﻿using App472.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App472.WebUI.Controllers
{
    public class BaseController : Controller
    {
        // Tab Return URLS
        // This is a dictionary of Guids and URLs, allowing back links to work even with multiple tabs open.
        //-------------------------------------------------------------------------------
        public TabReturnUrls TabUrls {
            get{
                return System.Web.HttpContext.Current.GetTabReturnUrls();
            }
        }
        // sets the key and value for the current request url, saving it to the session
        public Guid GenerateTabReturnUrl {
            get{
                Guid returnUrl = TabUrls.SetReturnUrl(Request.RawUrl);
                return returnUrl;
            }
        }
        // Retrieves the given url from the session.
        public string GetTabReturnUrl(string key)
        {
            if (string.IsNullOrEmpty(key)){
                return null;
            }
            string rawUrl = TabUrls.GetUrlString(new Guid(key));
            return rawUrl;
        }

        // Hook that executes before every controller action.
        //-------------------------------------------------------------------------------
        // See
        // https://stackoverflow.com/questions/26298356/execute-code-before-after-every-controller-action
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.TabReturnUrls = TabUrls.Tabs;
        }

        // Functionality relating to the user object stored in our session.
        //-------------------------------------------------------------------------------
        public bool IsLoggedInUser(){
            // Look in the session to see if BaseSessUser.UserID is null
            BaseSessUser sessUser = System.Web.HttpContext.Current.GetSessUser();
            bool isLoggedIn = sessUser.UserID != null;
            return isLoggedIn;
        }

        public BaseSessUser SessUser
        {
            get
            {
                if (IsLoggedInUser())
                {
                    return System.Web.HttpContext.Current.GetLoggedInSessUser();
                }
                else
                {
                    return System.Web.HttpContext.Current.GetNotLoggedInSessUser();
                }
            }
        }

        //-------------------------------------------------------------------------------
    }
}