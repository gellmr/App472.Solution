using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminEditProductViewModel : AdminViewModel
    {
        public Domain.Entities.Product Product { get; set; }
    }
}