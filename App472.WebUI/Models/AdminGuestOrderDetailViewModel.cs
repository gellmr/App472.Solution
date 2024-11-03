using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{

    public class AdminGuestOrderDetailViewModel: AdminBaseOrderDetailViewModel
    {
        public Guid GuestId { get; set; }
    }
}