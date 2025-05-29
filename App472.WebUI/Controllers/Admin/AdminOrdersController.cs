using App472.WebUI.Controllers.Admin;
using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using static App472.WebUI.Domain.Entities.Order;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminOrdersController : BaseAdminController
    {
        private IOrdersRepository orderRepo;

        private IGuestRepository guestRepo;
        private App472.WebUI.Infrastructure.Abstract.IFullUserRepository fullUserRepo;

        public AdminOrdersController(IOrdersRepository oRepo, IGuestRepository gRepo, App472.WebUI.Infrastructure.Abstract.IFullUserRepository fRepo)
        {
            orderRepo = oRepo;
            guestRepo = gRepo;
            fullUserRepo = fRepo;
            //fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); // cant do this in constructor for some reason
        }


        // Orders Backlog
        public ActionResult Index(string SortBy, bool? SortAscend, string Recent)
        {
            // Ensure query parameters are validated against strict regex
            // Allow alphabetical strings 5-15 chars long, or null/empty string.
            if(
                !MyExtensions.ValidateString(SortBy, "^[A-Za-z]{5,15}$") ||
                !MyExtensions.ValidateString(Recent, "^[A-Za-z]{5,15}$")
            ){
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
            }

            // Try to get query parameters from the session if they have not been set
            SortBy     = string.IsNullOrEmpty(SortBy) ? AdminSessUser.SortBy     : SortBy;
            SortAscend = (SortAscend == null)         ? AdminSessUser.SortAscend : SortAscend;
            Recent     = string.IsNullOrEmpty(Recent) ? AdminSessUser.Recent     : Recent;

            // If this is the first time page is requested and session value is empty, initialise to sort by order placed date.
            if (string.IsNullOrEmpty(SortBy)){
                SortBy = "OrderPlaced";
                SortAscend = false;
                Recent = null;
            }
            Order.OrderSortEnum sortEnum = Order.ParseOrderSortEnum(SortBy);
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers.ToList();
            IEnumerable<Order> orders = orderRepo.Orders; // default: No sort
            
            Dictionary<string, Models.Pair> Ascending = AdminOrdersViewModel.GetAscDefault();
            if(!string.IsNullOrEmpty(SortBy)){
                Models.Pair p = Ascending[SortBy];
                p.Asc = (bool)SortAscend;
            }

            switch (sortEnum){
                case OrderSortEnum.OrderID:         orders = orderRepo.Orders.OrderBy(order => order.ID);          if(!Ascending["OrderID"].Asc){ orders = orders.Reverse();}         break;
                case OrderSortEnum.Username:        orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestName);  if (!Ascending["Username"].Asc) { orders = orders.Reverse(); }     break;
                case OrderSortEnum.UserID:          orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestId);    if (!Ascending["UserID"].Asc) { orders = orders.Reverse(); }       break;
                case OrderSortEnum.AccountType:     orders = orderRepo.Orders.OrderBy(order => order.AccountType);      if (!Ascending["AccountType"].Asc) { orders = orders.Reverse(); }  break;
                case OrderSortEnum.Email:           orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestEmail);    if (!Ascending["Email"].Asc) { orders = orders.Reverse(); }        break;
                case OrderSortEnum.OrderPlaced:     orders = orderRepo.Orders.OrderBy(order => order.OrderPlacedDate);  if (!Ascending["OrderPlaced"].Asc) { orders = orders.Reverse(); }  break;
                //case OrderSortEnum.PaymentReceived: orders = orderRepo.Orders.OrderBy(order => order.PaymentReceived); break; // <-- still broken
                //case OrderSortEnum.Outstanding: orders = orderRepo.Orders.OrderBy(order => order.Outstanding); break; // <-- still broken
                case OrderSortEnum.ItemsOrdered:    orders = orderRepo.Orders.OrderBy(order => order.QuantityTotal);    if (!Ascending["ItemsOrdered"].Asc) { orders = orders.Reverse(); } break;
                case OrderSortEnum.Items:           orders = orderRepo.Orders.OrderBy(order => order.ItemString);       if (!Ascending["Items"].Asc) { orders = orders.Reverse(); } break;
                case OrderSortEnum.OrderStatus:     orders = orderRepo.Orders.OrderBy(order => order.OrderStatus);      if (!Ascending["OrderStatus"].Asc) { orders = orders.Reverse(); }  break;
                case OrderSortEnum.Edit:            orders = orderRepo.Orders.OrderBy(order => order.ID);          if (!Ascending["OrderID"].Asc) { orders = orders.Reverse(); } break;
                default: break;
            }

            // Persist the query parameters to admin session
            AdminSessUser.SortBy = SortBy;
            AdminSessUser.SortAscend = (bool)SortAscend;
            AdminSessUser.Recent = Recent;

            AdminOrdersViewModel vm = new AdminOrdersViewModel{
                CurrentPageNavText = AppNavs.OrdersNavText,
                Orders = orders,
                Guests = guestRepo.Guests,
                Users = fullUsers,
                Ascending = Ascending,
                Recent = SortBy
            };
            return View(vm);
        }
    }
}