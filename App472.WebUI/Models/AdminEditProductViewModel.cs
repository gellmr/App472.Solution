using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminEditProductViewModel : AdminViewModel
    {
        public Product Product { get; set; }
    }
}