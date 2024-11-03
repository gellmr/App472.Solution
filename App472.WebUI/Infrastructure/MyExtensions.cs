using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public static class MyExtensions
    {
        // Extend string for our convenience. Eg int? a = mystring.ToNullableInt()
        // See
        // https://stackoverflow.com/questions/45030/how-to-parse-a-string-into-a-nullable-int
        public static int? ToNullableInt(this string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
    }
}