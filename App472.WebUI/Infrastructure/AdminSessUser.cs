using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public class AdminSessUser
    {
        public AdminSessUser(){}

        public string SortBy { get; set; }    // Remembered across different requests.
        public bool? SortAscend { get; set; } // Remembered across different requests.
        public string Recent { get; set; }    // Remembered across different requests.
    }
}