using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    // encapsulate an AppUser with its EF objects
    public class FullUser
    {
        public App472.WebUI.Models.AppUser AppUser { get; set; }
        public IEnumerable<Domain.Entities.Order> Orders { get; set; }
    }
}