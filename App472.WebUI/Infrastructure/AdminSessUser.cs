using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public class AdminSessUser
    {
        public AdminSessUser() // constructor
        {
        }

        public string SortBy { get; set; } // remembered across different requests.
        public bool? SortAscend { get; set; } // remembered across different requests.
        public string Recent { get; set; } // remembered across different requests.
    }
}