using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminUserViewModel:AdminViewModel
    {
        public IEnumerable<App472.WebUI.Models.AppUser> Users { get; set;}
    }
}