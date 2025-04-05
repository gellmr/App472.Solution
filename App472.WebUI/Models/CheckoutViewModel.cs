using App472.WebUI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App472.WebUI.Models
{
    public class CheckoutViewModel // base class, used for (Cart...Checkout...Thankyou) pages
    {
        public Cart Cart { get; set; }
    }


    public class CheckoutButtonViewModel : CheckoutViewModel // view model to render the green Cart/Checkout button in the corner of the app
    {
        public string CheckoutButtonBackText { get; set; } // text to display on the button.
    }
    public class CartIndexViewModel: CheckoutViewModel // view model for Cart Index page.
    {
        public string ReturnUrl { get; set; } // for when we click to `continue shopping` it will go back.
    }
    public class CheckoutIndexViewModel : CheckoutViewModel // view model for Checkout Index page.
    {
        public ShippingDetails ShippingDetails { get; set; } // page has a `shipping details` form.
    }
}