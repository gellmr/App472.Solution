using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace App472.WebUI.Infrastructure
{
    public class TabReturnUrls
    {
        //  Key (guid)                   Value (string)
        // jn1i2n4k14bnlkj124bn12l4l2 : "Admin/Edit?OrderID=1",
        // n5o2n5oi3i5n35o32no532no53 : "Admin/Edit?OrderID=2",
        // n43o2n43on410n43ip4n3l1243 : "Admin/Index",
        // n4oiu12n4oi12n4o21n412n4p1 : "Admin/Users/Index",
        // 4hbo1n4oi1n4iop12n4p12n4ip : "Admin/Edit?UserId=1",
        // nl1kj2n4l12n4l12nl4n12lk41 : "Admin/Edit?UserId=2",

        public Dictionary<Guid, string> Tabs { get; set; }

        public TabReturnUrls()
        {
            Tabs = new Dictionary<Guid, string>();
        }

        public Guid SetReturnUrl(string returnUrl)
        {
            if (Tabs.Where(t => returnUrl.Equals(t.Value.ToString())).Count() == 0)
            {
                // Add new entry
                Guid key = Guid.NewGuid();
                Tabs.Add(key, returnUrl);
                return key;
            }
            else
            {
                // There is already an entry. Return the Guid.
                return Tabs.FirstOrDefault(t => t.Value.ToString() == returnUrl).Key;
            }
        }

        public string GetUrlString(Guid key)
        {
            string returnUrl = Tabs[key];
            return returnUrl;
        }
    }
}