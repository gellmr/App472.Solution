﻿using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
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
using static App472.WebUI.Domain.Entities.Order;

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
        public ViewResult Index(string SortBy = "", bool SortAscend = false, string Recent = null)
        {
            Order.OrderSortEnum sortEnum = Order.ParseOrderSortEnum(SortBy);
            fullUserRepo.AppUserManager = HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            IList<FullUser> fullUsers = fullUserRepo.FullUsers.ToList();
            IEnumerable<Order> orders = orderRepo.Orders; // default: No sort
            
            Dictionary<string,bool> Ascending = AdminOrdersViewModel.GetAscDefault();
            Ascending[SortBy] = SortAscend;

            switch (sortEnum){
                case OrderSortEnum.OrderID:         orders = orderRepo.Orders.OrderBy(order => order.OrderID);          if(Ascending["OrderID"]){ orders = orders.Reverse();}         break;
                case OrderSortEnum.Username:        orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestName);  if (Ascending["Username"]) { orders = orders.Reverse(); }     break;
                case OrderSortEnum.UserID:          orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestId);    if (Ascending["UserID"]) { orders = orders.Reverse(); }       break;
                case OrderSortEnum.AccountType:     orders = orderRepo.Orders.OrderBy(order => order.AccountType);      if (Ascending["AccountType"]) { orders = orders.Reverse(); }  break;
                case OrderSortEnum.Email:           orders = orderRepo.Orders.OrderBy(order => order.UserOrGuestEmail);    if (Ascending["Email"]) { orders = orders.Reverse(); }        break;
                case OrderSortEnum.OrderPlaced:     orders = orderRepo.Orders.OrderBy(order => order.OrderPlacedDate);  if (Ascending["OrderPlaced"]) { orders = orders.Reverse(); }  break;
                //case OrderSortEnum.PaymentReceived: orders = orderRepo.Orders.OrderBy(order => order.PaymentReceived); break; // <-- still broken
                case OrderSortEnum.ItemsOrdered:    orders = orderRepo.Orders.OrderBy(order => order.QuantityTotal);    if (Ascending["ItemsOrdered"]) { orders = orders.Reverse(); } break;
                case OrderSortEnum.OrderStatus:     orders = orderRepo.Orders.OrderBy(order => order.OrderStatus);      if (Ascending["OrderStatus"]) { orders = orders.Reverse(); }  break;
                default: break;
            }
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