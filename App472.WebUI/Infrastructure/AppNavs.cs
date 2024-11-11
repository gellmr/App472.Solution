using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public static class AppNavs
    {
        // Text appearing on nav buttons
        public static string ProductsNavText = "Edit Products";
        public static string OrdersNavText = "Orders Backlog";
        public static string UsersNavText = "User Accounts";

        // Routes for actions. Dont end them with a slash /
        public static string AdminOrders_Index = "/AdminOrders/Index";
        public static string AdminUserAcc_Index = "/AdminUserAcc/Index";
        public static string AdminUserOrder_Index = "/AdminUserOrder/Index";
        public static string AdminUserOrder_Detail = "/AdminUserOrder/Detail";
    }
}