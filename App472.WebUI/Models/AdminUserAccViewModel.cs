using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class AdminUserAccViewModel : AdminViewModel
    {
        public IEnumerable<Domain.Entities.Guest> Guests { get; set; }
        public IList<FullUser> FullUsers { get; set; }
    }
}