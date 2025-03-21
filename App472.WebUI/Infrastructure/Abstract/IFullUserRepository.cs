﻿using App472.WebUI.Infrastructure.DTO;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure.Abstract
{
    public interface IFullUserRepository{
        IEnumerable<FullUser> FullUsers {get;}
        AppUserManager AppUserManager { get; set; }
        LockoutUpdateResultDTO LockedOutUpdate(LockedOutUpdateDTO model);
        bool UsernameUpdate(UsernameUpdateDTO model);
        bool EmailUpdate(EmailUpdateDTO model);
        bool PhoneUpdate(PhoneUpdateDTO model);
    }
}