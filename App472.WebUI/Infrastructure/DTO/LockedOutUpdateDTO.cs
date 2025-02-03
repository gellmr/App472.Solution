using System;
using System.ComponentModel.DataAnnotations;

namespace App472.WebUI.Infrastructure.DTO
{
    public class LockedOutUpdateDTO
    {
        public Int32 UserID { get; set; }
        public bool Lock { get; set; }
    }

    public class LockoutUpdateResultDTO
    {
        public DateTime? Utc {get; set; }
        public Int32 Attempts { get; set; }
    }

    public class UsernameUpdateDTO
    {
        public Nullable<Int32> UserID { get; set; }
        public Nullable<Guid> GuestID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public bool IsGuest { get; set; }
    }

    public class EmailUpdateDTO
    {
        public Nullable<Int32> UserID { get; set; }
        public Nullable<Guid> GuestID { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsGuest { get; set; }
    }

    public class PhoneUpdateDTO
    {
        public Nullable<Int32> UserID { get; set; }
        public Nullable<Guid> GuestID { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public bool IsGuest { get; set; }
    }
}