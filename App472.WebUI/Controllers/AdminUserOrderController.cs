using App472.WebUI.Domain.Abstract;
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

        // List all orders belonging to a particular user.
        public ActionResult Index(string UserId, Nullable<Guid> guestId)
        {
            // BREAD CRUMBS
            // Starting from User Accounts... (Guest or FullUser)
            // AdminUserAcc_Index > AdminUserOrder_Index

            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel{
                CurrentPageNavText = AppNavs.UsersNavText
            };
            if (UserId != null){
                model.UserId  = UserId;
                model.UserName = AppNavs.GenUserName(UserId);
                model.Orders = repository.Orders.Where(o => o.UserID == UserId).OrderBy(o => o.OrderPlacedDate);
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
            return View(model);
        }

        public ActionResult Detail(Int32 OrderID, bool FromUserAccounts = false)
        {
            Order order = repository.Orders.Where(o => o.ID == OrderID).FirstOrDefault();
            string UserId = order.UserID;
            IEnumerable<OrderPayment> orderPayments = order.OrderPayments;
            IEnumerable<OrderedProduct> orderedProducts = order.OrderedProducts;
            if (order.OrderStatus == null){order.OrderStatus = Order.ParseShippingState(ShippingState.NotYetPlaced);}
            AdminOrderDetailViewModel model;
            string queryString = "";
            if (order.GuestID == null)
            {
                // user order
                model = new AdminOrderDetailViewModel{
                    UserId = UserId,
                    GuestId = null,
                    UserName = AppNavs.GenUserName(UserId),
                    OrderName = AppNavs.GenOrderName(order.ID)
                };
                queryString = "?UserId=" + model.UserId;
            }
            else{
                // guest order
                model = new AdminOrderDetailViewModel{
                    UserId = null,
                    GuestId = order.GuestID,
                    UserName = AppNavs.GenUserName(order.GuestID),
                    OrderName = AppNavs.GenOrderName(order.ID)
                };
                queryString = "?guestId=" + model.GuestId;
            }
            string shortUserName = MyExtensions.Truncate(model.UserName, MyExtensions.NavTruncLenth);
            if (FromUserAccounts == true)
            {
                // Coming from the User Accounts page
                BreadCrumb childLink2 = new BreadCrumb { URL = "",                                       BCLinkText = model.OrderName };
                BreadCrumb childLink1 = new BreadCrumb { URL = AppNavs.AdminUserOrder_Index+queryString, BCLinkText = shortUserName,        Child = childLink2 };
                BreadCrumb childLink0 = new BreadCrumb { URL = AppNavs.AdminUserAcc_Index,               BCLinkText = AppNavs.UsersNavText, Child = childLink1 };
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
            model.OrderID = OrderID;
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
            model.ReturnUrl = GenerateTabReturnUrl.ToString();
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