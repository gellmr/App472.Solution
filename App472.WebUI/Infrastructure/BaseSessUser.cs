using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    // Strongly typed object, to be stored in the session.
    // Has data representing the current user.
    public abstract class BaseSessUser
    {
        public Nullable<Int32> UserID { get; set; } // Matches their ID in the database. If they are not logged in, it will be null.

        public bool IsLoggedIn { get { return (UserID != null); } } // True if the user is logged in.

        public Guid GuestID { get; set; } // Guid generated at the start of the user session.

        public string Search{ get; set; } // The search string, remembered across different requests.
    }
}