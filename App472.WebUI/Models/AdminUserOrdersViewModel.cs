using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App472.WebUI.Infrastructure;

namespace App472.WebUI.Models
{
    public class AdminUserOrdersViewModel: AdminViewModel
    {
        public Int32 UserId {get; set; }
        public Nullable<Guid> GuestId { get; set; }
        public string UserName{get; set; }
        public BreadCrumb BCNavTrail { get; set; }

        public IEnumerable<Domain.Entities.Order> Orders {get; set;}

        public void AdminViewModel(){
            Orders = new List<Domain.Entities.Order>();
        }
    }
}