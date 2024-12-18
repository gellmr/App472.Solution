﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminOrdersViewModel:AdminViewModel
    {
        public IEnumerable<Domain.Entities.Order> Orders { get; set; }
        public IEnumerable<Domain.Entities.Guest> Guests { get; set; }
        public IEnumerable<App472.WebUI.Models.FullUser> Users { get; set; }
    }
}