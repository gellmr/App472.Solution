using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminOrdersViewModel:AdminViewModel
    {
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Guest> Guests { get; set; }
        public IEnumerable<App472.WebUI.Models.FullUser> Users { get; set; }
    }
}