using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static App472.WebUI.Domain.Entities.Order;
using System.Web.UI.WebControls;

namespace App472.WebUI.Models
{
    public class AdminOrdersViewModel:AdminViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<App472.WebUI.Models.FullUser> Users { get; set; }
        public Dictionary<string,bool> Ascending { get; set; }
        public static Dictionary<string, bool> GetAscDefault(){
            bool boolDef = false;
            return new Dictionary<string, bool>{
                {"OrderID",boolDef },
                {"Username",boolDef },
                {"UserID",boolDef },
                {"AccountType",boolDef },
                {"Email",boolDef },
                {"OrderPlaced",boolDef },
                {"PaymentReceived",boolDef },
                {"ItemsOrdered",boolDef },
                {"OrderStatus",boolDef },
            };
        }
        public string Recent { get; set; }
    }
}