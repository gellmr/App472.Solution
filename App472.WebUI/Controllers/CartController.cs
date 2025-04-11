using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using App472.WebUI.Infrastructure;
using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using System.Net;

namespace App472.WebUI.Controllers
{
    public class CartController : BaseController
    {
        private IProductsRepository repository;
        private IOrderProcessor orderProcessor;
        private IOrdersRepository orderRepo;
        private IGuestRepository guestRepo;

        public CartController(IProductsRepository repo, IOrderProcessor proc, IOrdersRepository orepo, IGuestRepository grepo){
            repository = repo;
            orderProcessor = proc;
            orderRepo = orepo;
            guestRepo = grepo;
        }

        /*
         * Test this action from browser console with the following ajax call.
         $.ajax( {url: "/Cart/AddToCart", type: 'POST', data: {inStockProductId:2, returnUrl:"/Water%20Sports"} });
         */
        public ActionResult AddToCart(Cart cart, int inStockProductId, string returnUrl)
        {
            if (!MyExtensions.ValidateReturnUrl(returnUrl)){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }
            InStockProduct product = repository.InStockProducts
                .FirstOrDefault(p => p.ID == inStockProductId);
            if ( product != null) {
                cart.AddItem(product, 1);
            }
            // pass returnUrl to the cart index page - this tells the Continue Shopping button to take us
            // back to the page and category we were on.
            return RedirectToAction("Index", new { returnUrl }); 
        }

        /*
         * Test this action from browser console with the following ajax call.
         $.ajax( {url: "/Cart/RemoveFromCart", type: 'POST', data: {inStockProductId:2, returnUrl:"/Water%20Sports"} });
         */
        public ActionResult RemoveFromCart(Cart cart, int inStockProductId, string returnUrl)
        {
            if (!MyExtensions.ValidateReturnUrl(returnUrl)){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }
            InStockProduct product = repository.InStockProducts.FirstOrDefault(p => p.ID == inStockProductId);
            if(product != null){
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        /*
         * Test this action from browser console with the following ajax call.
         $.ajax( {url: "/Cart/Index", type: 'GET', data: {returnUrl: "/Water%20Sports"} });
         */
        public ActionResult Index(Cart cart, string returnUrl)
        {
            if (!MyExtensions.ValidateReturnUrl(returnUrl)){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }
            return View(new CartIndexViewModel{
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        /*
         * Test this action from browser console with the following ajax call.
         $.ajax( {url: "/Cart/Summary", type: 'GET', data: {requestPath: "/Cart/Index"} });
         $.ajax( {url: "/Cart/Summary", type: 'GET', data: {requestPath: "/Cart/Checkout"} });
         */
        public PartialViewResult Summary(Cart cart, string requestPath) // called from NavBarContent to load a page fragment. cart is automatically pulled from the session.
        {
            // Here we only check if requestPath exactly equals /Cart/Index, to set the text in our Cart|Checkout button.
            // Any other value (including dangerous string) we dont care about.

            // The "Cart" button text changes to "Checkout" when you are on cart page, to advance you forward.
            // On the checkout page it says "Cart" to take you back.
            string backText = (requestPath == "/Cart/Index") ? "Checkout" : "Cart";

            return PartialView(new CheckoutButtonViewModel { Cart = cart, CheckoutButtonBackText = backText }); // render Summary.cshtml within NavBarContent
        }

        public ViewResult Checkout()
        {
            return View(new CheckoutIndexViewModel{
                ShippingDetails = new ShippingDetails()
            });
        }


        /*
         * Test this action by adding an item to your cart, and then go to checkout page.
         * Execute the following ajax from your browser console.
           $.ajax( {url: "/Cart/Checkout", type: 'POST', data: {

	        "ShippingDetails.FirstName": "Luna",
	        "ShippingDetails.LastName": "DeGras",

	        "ShippingDetails.Line1": "22 Skyline Cr",
	        "ShippingDetails.Line2": "empty line 2",
	        "ShippingDetails.Line3": "empty line 3",

	        "ShippingDetails.City": "Mirrabooka",
	        "ShippingDetails.State": "Western Australia",
	        "ShippingDetails.Country": "Australia",
	        "ShippingDetails.Zip": "6061",

	        "ShippingDetails.Email": "degras@thinktank.org",
	        "ShippingDetails.GiftWrap": "true",
        }});
        */
        [HttpPost]
        public ActionResult Checkout(Cart cart, CheckoutIndexViewModel viewModel)
        {
            if (!MyExtensions.ValidateShippingDetails(viewModel.ShippingDetails)){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }

            if(cart.Lines.Count() == 0){
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                BaseSessUser sessUser = SessUser; // get the strongly typed object from session representing our user, who may be logged in or not.
                if (sessUser.IsLoggedIn)
                {
                    // The logged in user is placing an order.
                    LoggedInSessUser loggedInSessUser = (LoggedInSessUser)sessUser;
                    
                    // Look in the database for our current user
                    var userManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
                    AppUser user = userManager.FindById( sessUser.UserID.ToString() );

                    // Ready to save the order.
                    SaveOrder(cart, viewModel.ShippingDetails, user.Id, null);
                }
                else
                {
                    // User is not logged in. Can still place an order as guest.
                    NotLoggedInSessUser notLoggedInSessUser = (NotLoggedInSessUser)sessUser;

                    //ModelState.AddModelError("", "Please login before placing an order!");
                    // orderProcessor.ProcessOrder(cart, shippingDetails); // Send an email

                    // Check if we have an existing GuestID for the user email address.
                    notLoggedInSessUser.GuestID = guestRepo.GuestExists(viewModel.ShippingDetails.Email) ?? notLoggedInSessUser.GuestID;
                    Nullable<Guid> guestID = notLoggedInSessUser.GuestID;

                    // Create a database record using the guest id.
                    SaveOrder(cart, viewModel.ShippingDetails, null, guestID);
                }

                cart.Clear();
                return View("Completed");
            }
            else
            {
                // Model has some errors
                return View(viewModel);
            }
        }

        private void SaveOrder(Cart cart, ShippingDetails shippingDetails, string userId, Nullable<Guid> guestId)
        {
            Order order1 = new Order(); // create domain object

            string shipAddress = Order.ParseAddress(shippingDetails);
            DateTimeOffset now = DateTimeOffset.Now;

            order1.GuestID = guestId; // null if user is logged in when placing order
            order1.UserID = userId;

            order1.OrderPlacedDate = now;
            order1.PaymentReceivedDate = null;
            order1.ReadyToShipDate = null;
            order1.ShipDate = null;
            order1.ReceivedDate = null;
            order1.BillingAddress = shipAddress;
            order1.ShippingAddress = shipAddress;
            order1.OrderStatus = Order.ParseShippingState(ShippingState.OrderPlaced);

            if (guestId != null){
                // User is not logged in. Create order as guest.
                // Create a guest record in the database using the guid.
                Guest guest = new Guest{
                    ID = guestId,
                    Email = shippingDetails.Email,
                    FirstName = shippingDetails.FirstName,
                    LastName = shippingDetails.LastName
                };
                guestRepo.SaveGuest(guest);
            }
            // create an ordered product for each cart line
            foreach (var line in cart.Lines)
            {
                //var subtotal = line.Product.Price * line.Quantity;
                OrderedProduct op1 = new OrderedProduct();
                op1.InStockProduct = line.InStockProduct;
                op1.InStockProductID = line.InStockProduct.ID;

                op1.OrderID = order1.ID;
                op1.Order = order1;
                op1.Quantity = line.Quantity;
                order1.OrderedProducts.Add(op1);
            }
            orderRepo.SaveOrder(order1);
        }
    }
}