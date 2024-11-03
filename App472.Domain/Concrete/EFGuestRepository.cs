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
                List<Guest> guests = new List<Guest>();
                IEnumerable<Order> guestOrders = context.Orders.Where( o => o.GuestID != null);
                foreach (Order o in guestOrders){
                    Guest guest = context.Guests.FirstOrDefault( g => g.Id == o.GuestID );
                    guests.Add(
                        new Guest{
                            Id = o.GuestID,
                            Email = guest.Email,
                            FirstName = guest.FirstName,
                            LastName = guest.LastName
                        }
                    );
                }
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
    }
}
