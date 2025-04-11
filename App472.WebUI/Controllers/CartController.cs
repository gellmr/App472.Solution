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
        protected List<string> Whitelist = new List<string>{
            OkUrls.StorePage,
            OkUrls.ChessCat,
            OkUrls.SoccerCat,
            OkUrls.WaterSportsCat,
            OkUrls.CartCheckout
        };

        public CartController(IProductsRepository repo, IOrderProcessor proc, IOrdersRepository orepo, IGuestRepository grepo){
            repository = repo;
            orderProcessor = proc;
            orderRepo = orepo;
            guestRepo = grepo;
        }
        public RedirectToRouteResult AddToCart(Cart cart, int inStockProductId, string returnUrl){
            InStockProduct product = repository.InStockProducts
                .FirstOrDefault(p => p.ID == inStockProductId);
            if ( product != null) {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int inStockProductId, string returnUrl){
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
            if ( !(
                MyExtensions.ValidateString(returnUrl, OkUrls.ReturnUrl)
                &&
                MyExtensions.ValidateStringAgainst(returnUrl, Whitelist)
            ))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }
            return View(new CartIndexViewModel{
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart, string requestPath) // called from NavBarContent to load a page fragment. cart is automatically pulled from the session.
        {
            string backText = (requestPath == "/Cart/Index") ? "Checkout" : "Cart";
            return PartialView(new CheckoutButtonViewModel { Cart = cart, CheckoutButtonBackText = backText }); // render Summary.cshtml within NavBarContent
        }

        public ViewResult Checkout()
        {
            return View(new CheckoutIndexViewModel{
                ShippingDetails = new ShippingDetails()
            });
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, CheckoutIndexViewModel viewModel)
        {
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