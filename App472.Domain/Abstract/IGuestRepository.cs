using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App472.Domain.Abstract
{
    public interface IGuestRepository
    {
        IEnumerable<Guest> Guests { get; }
        void SaveGuest(Guest guest);

        Nullable<Guid> GuestExists(string email);
    }
}
