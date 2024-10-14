using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public static class MyExtensions
    {
        public static string TRUrlsSessKeyName = "__TabReturnUrls";

        // Extend HttpContext for our convenience
        // See
        // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        // https://stackoverflow.com/questions/560084/session-variables-in-asp-net-mvc
        public static TabReturnUrls GetTabReturnUrls(this HttpContext current)
        {
            return current != null ? (TabReturnUrls)current.Session[MyExtensions.TRUrlsSessKeyName] : null;
        }
    }
}