using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static App472.WebUI.Domain.Entities.Order;
using System.Web.UI.WebControls;

namespace App472.WebUI.Models
{
    public class Pair{ public bool Asc {get;set;} public string Align {get;set; } }

    public class AdminOrdersViewModel:AdminViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<App472.WebUI.Models.FullUser> Users { get; set; }
        public Dictionary<string, Pair> Ascending { get; set; }
        
        public static Dictionary<string, Pair> GetAscDefault(){
            bool boolDef = true;
            return new Dictionary<string, Pair>{
                {"OrderID",new Pair{Asc=boolDef,Align="text-center"} },
                {"Username",new Pair{Asc=boolDef,Align="text-left"} },
                {"UserID",new Pair{Asc=boolDef,Align="text-left"} },
                {"AccountType",new Pair{Asc=boolDef,Align="text-center"} },
                {"Email",new Pair{Asc=boolDef,Align="text-left"} },
                {"OrderPlaced",new Pair{Asc=!boolDef,Align="text-left"} },
                {"PaymentReceived",new Pair{Asc=boolDef,Align="text-center"} },
                {"ItemsOrdered",new Pair{Asc=boolDef,Align="text-center"} },
                {"Items",new Pair{Asc=boolDef,Align="text-left"} },
                {"OrderStatus",new Pair{Asc=boolDef,Align="text-center"} }

            };
        }
        public string Recent { get; set; }
    }
}