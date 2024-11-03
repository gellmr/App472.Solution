using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    // Static methods to extend the session object for convenience.
    public static class SessExtensions
    {
        public static string TRUrlsSessKeyName = "__TabReturnUrls";

        // Extend HttpContext for our convenience
        // See
        // https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/extension-methods
        // https://stackoverflow.com/questions/560084/session-variables-in-asp-net-mvc
        public static TabReturnUrls GetTabReturnUrls(this HttpContext current)
        {
            return current != null ? (TabReturnUrls)current.Session[SessExtensions.TRUrlsSessKeyName] : null;
        }

        public static string SessUserKeyName = "__SessUser";
        public static BaseSessUser GetSessUser(this HttpContext current)
        {
            return current != null ? (BaseSessUser)current.Session[SessExtensions.SessUserKeyName] : null;
        }
        public static LoggedInSessUser GetLoggedInSessUser(this HttpContext current)
        {
            return current != null ? (LoggedInSessUser)current.Session[SessExtensions.SessUserKeyName] : null;
        }
        public static NotLoggedInSessUser GetNotLoggedInSessUser(this HttpContext current)
        {
            return current != null ? (NotLoggedInSessUser)current.Session[SessExtensions.SessUserKeyName] : null;
        }

        public static string GuestIDSessKeyName = "__GuestID";
        public static Guid GuestIDSess(this HttpContext current)
        {
            return (Guid)current.Session[GuestIDSessKeyName];
        }
    }
}