using System;

namespace App472.WebUI.Infrastructure.DTO
{
    public class LockedOutUpdateDTO
    {
        public Int32 UserID { get; set; }
        public bool Lock { get; set; }
    }
}