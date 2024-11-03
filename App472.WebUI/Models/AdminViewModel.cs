using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminViewModel : BaseViewModel
    {
        // Links shown at the top of the Admin pages
        // LinkText, URL route value
        public Dictionary<string, string> Links = new Dictionary<string, string>()
        {
            {"Edit Products", "/Admin/Index"},
            {"User Orders", "/AdminUser/Index"}
        };

        public string LinkText { get; set; }
    }
}