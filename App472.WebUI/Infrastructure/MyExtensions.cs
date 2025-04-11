using App472.WebUI.Domain.Entities;
using PCRE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;

namespace App472.WebUI.Infrastructure
{
    public static class OkUrls
    {
        // allow alphanumeric, right slash, space, dash, percent sign, 1-80 characters
        public static string ReturnUrl{ get { return
            "^[A-Za-z0-9\\/\\s\\-\\%]{1,80}$"
        ;}}

        public static string StorePage{ get { return
            //"^\\/$"
            "^\\/(page[\\d]{1,3})?$"
        ;}}
        public static string ChessCat{ get { return
            //"^\\/Chess$"
            "^\\/Chess(\\/page[\\d]{1,3})?$"
        ;}}
        public static string SoccerCat{ get { return
            //"^\\/Soccer$"
            "^\\/Soccer(\\/page[\\d]{1,3})?$"
        ;}}
        public static string WaterSportsCat{ get { return
            //"^\\/Water\\%20Sports$"
            "^\\/Water\\%20Sports(\\/page[\\d]{1,3})?$"
        ;}}
        public static string CartCheckout{ get { return
            //"^\\/Cart\\/Checkout$"
            "^\\/Cart\\/Checkout(\\/page[\\d]{1,3})?$"
        ;}}
    }
    
    public static class OkInputs{
        public static string Name{ get { return // alphanumeric, space, dash. 1-30 characters
            "^[A-Za-z0-9\\s\\-]{1,30}$"
        ;}}
    }

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
            int substrLen = maxLength - 3;
            return value.Length <= maxLength ? value : value.Substring(0, substrLen) + "...";
        }

        public static Int32 NavTruncLenth = 14;
        public static Int32 GuidTruncLenth = 8;

        // Get list of errors in a ModelState
        public static List<string> ModelErrors(this System.Web.Mvc.ModelStateDictionary ModelState){
            var errors = new List<string>();
            foreach (var state in ModelState){
                foreach (var error in state.Value.Errors){
                    errors.Add(error.ErrorMessage);
                }
            }
            return errors;
        }

        // Extend string class with a validation method that checks the given string against a given regex.
        //   Validate the given string against a regex, using .NET PCRE
        //   We are only looking for malicious input.
        //   Null or empty strings are valid.
        public static bool ValidateString(this string input, string validationPattern)
        {
            if (!string.IsNullOrEmpty(input))
            {
                var regex = new PcreRegex(validationPattern);
                bool isValid = regex.IsMatch(input);
                if (!isValid)
                {
                    return false; // does not match the given regex
                }
            }
            // matches the given regex -OR- input string was null or empty. (not an attack)
            return true;
        }

        // Validate against a list of regex patterns.
        // If the given input string is null or empty, all checks will pass.
        public static bool ValidateStringAgainst(this string input, List<string> regexList)
        {
            if (string.IsNullOrEmpty(input)){
                return true; // null or empty strings are ok
            }
            foreach(string validationPattern in regexList)
            {
                var regex = new PcreRegex(validationPattern);
                bool isMatch = regex.IsMatch(input);
                if (isMatch){
                    return true; // found a match
                }
            }
            return false; // none of the given regex matched the input string.
        }

        public static bool ValidateReturnUrl(this string returnUrl)
        {
            // return true if the given url matches our ok return url pattern,
            // AND is on the whitelist.
            return (
                MyExtensions.ValidateString(returnUrl, OkUrls.ReturnUrl)
                &&
                MyExtensions.ValidateStringAgainst(returnUrl, Whitelist.URLs)
            );
        }

        public static bool ValidateShippingDetails(this ShippingDetails shipping)
        {
            return (
                MyExtensions.ValidateString(shipping.FirstName, OkInputs.Name) &&
                MyExtensions.ValidateString(shipping.LastName, OkInputs.Name)
            );
        }
    }
}