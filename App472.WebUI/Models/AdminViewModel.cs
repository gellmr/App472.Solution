using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App472.WebUI.Infrastructure;

namespace App472.WebUI.Models
{
    public class AdminViewModel : BaseViewModel
    {
        // Links shown at the top of the Admin pages
        // LinkText, URL route value
        public Dictionary<string, string> AdminNavLinks = new Dictionary<string, string>()
        {
            // TODO - remove this and use AppNavs
            // TODO - remove this and use AppNavs
            // TODO - remove this and use AppNavs
            {AppNavs.ProductsNavText, AppNavs.AdminProducts_Index},
            {AppNavs.OrdersNavText, AppNavs.AdminOrders_Index},
            {AppNavs.UsersNavText, AppNavs.AdminUserAcc_Index}
        };

        public string CurrentPageNavText { get; set; }
    }
}