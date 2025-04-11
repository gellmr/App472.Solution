using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public static class Whitelist
    {
        public static List<string> URLs = new List<string>{
            OkUrls.StorePage,
            OkUrls.ChessCat,
            OkUrls.SoccerCat,
            OkUrls.WaterSportsCat,
            OkUrls.CartCheckout
        };
    }
}