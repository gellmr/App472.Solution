using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    // Strongly typed object
    // Session data representing the current user who is not logged in.
    public class NotLoggedInSessUser : BaseSessUser
    {
        public NotLoggedInSessUser() : base() {
            GuestID = Guid.NewGuid(); // Generate the Guid for the guest id of this user session.
        }
    }
}