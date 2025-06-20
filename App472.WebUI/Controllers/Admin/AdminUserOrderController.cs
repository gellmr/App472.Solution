﻿using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminUserOrderController : BaseController
    {
        private IOrdersRepository repository;

        public AdminUserOrderController(IOrdersRepository repo)
        {
            this.repository = repo;
        }

        public ActionResult Guest(string ID)
        {
            Nullable<Guid> guestId = MyExtensions.ToNullableGuid(ID);
            AdminUserOrdersViewModel model = ServeIndex(null, guestId);
            return View("Index", model);
        }
        
        public ActionResult Index(string ID){
            AdminUserOrdersViewModel model = ServeIndex(ID, null);
            return View(model);
        }

        // List all orders belonging to a particular user.
        private AdminUserOrdersViewModel ServeIndex(string ID, Nullable<Guid> guestId)
        {
            // BREAD CRUMBS
            // Starting from User Accounts... (Guest or FullUser)
            // AdminUserAcc_Index -> AdminUserOrder

            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel{
                CurrentPageNavText = AppNavs.UsersNavText
            };
            if (ID != null){
                model.UserId  = ID;
                model.UserName = AppNavs.GenUserName(ID);
                model.Orders = repository.Orders.Where(o => o.UserID == ID).OrderBy(o => o.OrderPlacedDate);
            }
            else if (guestId != null)
            {
                model.GuestId = guestId;
                model.UserName = AppNavs.GenUserName(guestId);
                model.Orders = repository.Orders.Where(o => o.GuestID == guestId).OrderBy(o => o.OrderPlacedDate);
            }
            string shortUserName = MyExtensions.Truncate(model.UserName, MyExtensions.NavTruncLenth);
            BreadCrumb childLink1 = new BreadCrumb { URL = "",                         BCLinkText = shortUserName };
            BreadCrumb childLink0 = new BreadCrumb { URL = AppNavs.AdminUserAcc_Index, BCLinkText = AppNavs.UsersNavText, Child = childLink1 };
            model.BCNavTrail = childLink0;
            return model;
        }

        public ActionResult Detail(Int32 ID, bool FromUserAccounts = false)
        {
            Order order = repository.Orders.Where(o => o.ID == ID).FirstOrDefault();
            string UserId = order.UserID;
            IEnumerable<OrderPayment> orderPayments = order.OrderPayments;
            IEnumerable<OrderedProduct> orderedProducts = order.OrderedProducts;
            if (order.OrderStatus == null){order.OrderStatus = Order.ParseShippingState(ShippingState.NotYetPlaced);}
            AdminOrderDetailViewModel model;
            string idPortion = "";
            if (order.GuestID == null)
            {
                // user order
                model = new AdminOrderDetailViewModel{
                    UserId = UserId,
                    GuestId = null,
                    UserName = AppNavs.GenUserName(UserId),
                    OrderName = AppNavs.GenOrderName(order.ID)
                };
                idPortion = "/" + model.UserId;
            }
            else{
                // guest order
                model = new AdminOrderDetailViewModel{
                    UserId = null,
                    GuestId = order.GuestID,
                    UserName = AppNavs.GenUserName(order.GuestID),
                    OrderName = AppNavs.GenOrderName(order.ID)
                };
                idPortion = "/Guest/" + model.GuestId;
            }
            string shortUserName = MyExtensions.Truncate(model.UserName, MyExtensions.NavTruncLenth);
            if (FromUserAccounts == true)
            {
                // Coming from the User Accounts page
                BreadCrumb childLink2 = new BreadCrumb { URL = "",                                 BCLinkText = model.OrderName };
                BreadCrumb childLink1 = new BreadCrumb { URL = AppNavs.AdminUserOrder+idPortion,   BCLinkText = shortUserName,        Child = childLink2 };
                BreadCrumb childLink0 = new BreadCrumb { URL = AppNavs.AdminUserAcc_Index,         BCLinkText = AppNavs.UsersNavText, Child = childLink1 };
                model.BCNavTrail = childLink0;
                model.CurrentPageNavText = AppNavs.UsersNavText;
            }
            else
            {
                // Coming from the Order Backlog page
                BreadCrumb childLink1 = new BreadCrumb { URL = "",                        BCLinkText = model.OrderName };
                BreadCrumb childLink0 = new BreadCrumb { URL = AppNavs.AdminOrders_Index, BCLinkText = AppNavs.OrdersNavText, Child = childLink1 };
                model.BCNavTrail = childLink0;
                model.CurrentPageNavText = AppNavs.OrdersNavText;
            }
            model.OrderID = ID;
            model.OrderPayments = orderPayments.ToList();
            model.OrderedProducts = orderedProducts.ToList();
            model.OrderPlacedDate = order.OrderPlacedDate;
            model.PaymentReceivedDate = order.PaymentReceivedDate;
            model.ReadyToShipDate = order.ReadyToShipDate;
            model.ShipDate = order.ShipDate;
            model.ReceivedDate = order.ReceivedDate;
            model.BillingAddress = order.BillingAddress;
            model.ShippingAddress = order.ShippingAddress;
            model.OrderStatus = Order.ParseShippingState(order.OrderStatus);
            return View(model);
        }

        // See
        // https://stackoverflow.com/questions/39187756/asp-net-mvc-model-wont-bind-on-ajax-post
        public class DetailUpdateDTO{ public Int32 ProductID {get;set;} public Int32 OrderID {get;set;} }
        public class ProductLineUpdateDTO {
            public Int32 ProductID { get; set; }
            public Int32 OrderID{ get; set; }
            public Int32 NewQty { get; set; }
        }

        [HttpPost]
        public HttpStatusCodeResult DeleteProduct(DetailUpdateDTO model)
        {
            repository.DeleteOrderedProduct(model.ProductID, model.OrderID);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public JsonResult ProductLineUpdate(ProductLineUpdateDTO model){
            repository.UpdateOrderedProductLineQuantity(model.ProductID, model.OrderID, model.NewQty);
            Order order = repository.Orders.Where(o => o.ID == model.OrderID).FirstOrDefault();
            var data = order.GetUpdateDTO();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class ShipStatusDTO { public Int32 OrderID { get; set; } public Int32 OrderStatus { get; set; } public Nullable<Decimal> PaymentAmount {get;set;} }
        [HttpPost]
        public HttpStatusCodeResult SetShipping(ShipStatusDTO model){
            try{
                repository.UpdateShippingStatus(model.OrderID, model.OrderStatus, model.PaymentAmount);
            }catch{
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}