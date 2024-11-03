using App472.Domain.Abstract;
using App472.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App472.Domain.Concrete
{
    public class EFGuestRepository : IGuestRepository
    {
        private EFDBContext context = new EFDBContext();

        public IEnumerable<Guest> Guests{
            get{
                IEnumerable<Guest> guests = context.Guests.Where(g => g.Id != null && !string.IsNullOrEmpty(g.Email) );
                return guests;
            }
        }

        public void SaveGuest(Guest guest)
        {
            // Could we possibly reduce this to one database call?
            if (context.Guests.Any(g => g.Id == guest.Id)) // first call
            {
                // record already exists. Update
                Guest dbEntry = context.Guests.FirstOrDefault(g => g.Id == guest.Id); // second call
                dbEntry.Id = guest.Id;
                dbEntry.Email = guest.Email;
                dbEntry.FirstName = guest.FirstName;
                dbEntry.LastName = guest.LastName;
                context.SaveChanges();
            }
            else
            {
                // create new record
                context.Guests.Add(guest);
                context.SaveChanges();
            }
        }

        public Nullable<Guid> GuestExists(string email){
            Guest guest = context.Guests.FirstOrDefault(g => g.Email.Equals(email));
            if (guest != null){
                return (Nullable<Guid>)guest.Id;
            }
            return null;
        }
    }
}
