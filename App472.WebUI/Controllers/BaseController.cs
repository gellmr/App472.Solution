using App472.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App472.WebUI.Controllers
{
    public class BaseController : Controller
    {
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

        // See
        // https://stackoverflow.com/questions/26298356/execute-code-before-after-every-controller-action
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.TabReturnUrls = TabUrls.Tabs;
        }
    }
}