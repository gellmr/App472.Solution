using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App472.Domain.Entities;

namespace App472.WebUI.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart{get; set; }
        public string ReturnUrl { get; set; }
    }
}