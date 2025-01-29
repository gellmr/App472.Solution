using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;

namespace App472.WebUI.Domain.Abstract
{
    public interface IGuestRepository
    {
        IEnumerable<Guest> Guests { get; }
        void SaveGuest(Guest guest);
        Nullable<Guid> GuestExists(string email);
        bool EmailUpdate(Infrastructure.DTO.EmailUpdateDTO model);
    }
}