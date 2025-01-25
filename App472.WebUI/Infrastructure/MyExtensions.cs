using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public static class MyExtensions
    {
        // Extend string for our convenience. Eg Guid? a = mystring.ToNullableGuid()
        // See
        // https://stackoverflow.com/questions/45030/how-to-parse-a-string-into-a-nullable-int
        public static Nullable<Guid> ToNullableGuid(this string s)
        {
            Guid g;
            if (Guid.TryParse(s, out g)) return g;
            return null;
        }

        // https://stackoverflow.com/questions/2776673/how-do-i-truncate-a-net-string
        public static string Truncate(this string value, int maxLength){
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + "...";
        }

        public static Int32 NavTruncLenth = 14;
        public static Int32 GuidTruncLenth = 8;
    }
}