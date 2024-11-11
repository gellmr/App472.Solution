using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public class BreadCrumb
    {
        public BreadCrumb Child{ get; set; }
        public string URL { get; set;}
        public string BCLinkText{ get; set;}
    }
}