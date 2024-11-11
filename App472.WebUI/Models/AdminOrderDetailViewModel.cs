using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App472.WebUI.Infrastructure;

namespace App472.WebUI.Models
{
    public class AdminOrderDetailViewModel: AdminBaseOrderDetailViewModel
    {
        public Nullable<Int32> UserId { get; set; }
        public Nullable<Guid> GuestId { get; set; }
        public string UserName { get; set; }
        public string OrderName { get; set; }
        public BreadCrumb BCNavTrail { get; set; }
    }
}