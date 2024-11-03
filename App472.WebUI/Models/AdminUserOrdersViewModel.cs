using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminUserOrdersViewModel: AdminViewModel
    {
        public Int32 UserId {get; set; }
        public Nullable<Guid> GuestId { get; set; }

        public IEnumerable<Domain.Entities.Order> Orders {get; set;}

        public void AdminViewModel(){
            Orders = new List<Domain.Entities.Order>();
        }
    }
}