using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using static App472.Domain.Entities.Order;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminOrdersController : BaseController
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
        public ViewResult Index(string SortBy = "")
        {
            Order.OrderSortEnum sortEnum = Order.ParseOrderSortEnum(SortBy);
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers.ToList();
            IEnumerable<Domain.Entities.Order> orders = orderRepo.Orders; // default: No sort
            switch (sortEnum){
                case OrderSortEnum.OrderID:         orders = orderRepo.Orders.OrderBy(order => order.OrderID); break;
                // oh no... the username is in my AppUser object which is in the ID database. How do I look up the username?
                //case OrderSortEnum.Username:        orders = orderRepo.Orders.OrderBy(order => order.Username); break;        // <-- here
                case OrderSortEnum.UserID:          orders = orderRepo.Orders.OrderBy(order => order.UserID); break;
                //case OrderSortEnum.AccountType:     orders = orderRepo.Orders.OrderBy(order => order.AccountType); break;     // <-- and here
                //case OrderSortEnum.Email:           orders = orderRepo.Orders.OrderBy(order => order.Email); break;           // <-- and here
                //case OrderSortEnum.OrderPlaced:     orders = orderRepo.Orders.OrderBy(order => order.OrderPlaced); break;     // <-- and here
                //case OrderSortEnum.PaymentReceived: orders = orderRepo.Orders.OrderBy(order => order.PaymentReceived); break; // <-- and here
                //case OrderSortEnum.ItemsOrdered:    orders = orderRepo.Orders.OrderBy(order => order.ItemsOrdered); break;    // <-- and here
                case OrderSortEnum.OrderStatus:     orders = orderRepo.Orders.OrderBy(order => order.OrderStatus); break;
                default: break;
            }
            AdminOrdersViewModel vm = new AdminOrdersViewModel{
                CurrentPageNavText = AppNavs.OrdersNavText,
                Orders = orders,
                Guests = guestRepo.Guests,
                Users = fullUsers
            };
            return View(vm);
        }
    }
}