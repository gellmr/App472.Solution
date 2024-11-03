using App472.Domain.Abstract;
using App472.Domain.Entities;
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

        public ActionResult Index(int? UserId, Nullable<Guid> guestId)
        {
            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel{
                LinkText = "Edit Users",
            };
            if (UserId != null){
                model.UserId  = (Int32)UserId;
                model.Orders = repository.Orders.Where(o => o.UserID == UserId).OrderBy(o => o.OrderID);
            }
            else if (guestId != null)
            {
                model.GuestId = guestId;
                model.Orders = repository.Orders.Where(o => o.GuestID == guestId).OrderBy(o => o.OrderPlacedDate);
            }
            return View(model);
        }

        public ActionResult Detail(Int32 OrderID)
        {
            Order order = repository.Orders.Where(o => o.OrderID == OrderID).FirstOrDefault();
            IEnumerable<OrderedProduct> orderedProducts = order.OrderedProducts;
            if (order.OrderStatus == null){order.OrderStatus = Order.ParseShippingState(ShippingState.NotYetPlaced);}

            AdminBaseOrderDetailViewModel model;
            string viewName = "Detail";
            if (order.GuestID == null){
                // user order
                model = new AdminUserOrderDetailViewModel{UserId = (Int32)order.UserID};
            }
            else{
                // guest order
                model = new AdminGuestOrderDetailViewModel{GuestId = (Guid)order.GuestID};
                viewName = "GuestDetail";
            }
            model.LinkText = "Edit Users";
            model.OrderID = OrderID;
            model.OrderedProducts = orderedProducts.ToList();
            model.OrderPlacedDate = order.OrderPlacedDate;
            model.PaymentReceivedDate = order.PaymentReceivedDate;
            model.ReadyToShipDate = order.ReadyToShipDate;
            model.ShipDate = order.ShipDate;
            model.ReceivedDate = order.ReceivedDate;
            model.BillingAddress = order.BillingAddress;
            model.ShippingAddress = order.ShippingAddress;
            model.OrderStatus = App472.Domain.Entities.Order.ParseShippingState(order.OrderStatus);
            model.ReturnUrl = GenerateTabReturnUrl.ToString();
            return View(viewName, model);
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
            Order order = repository.Orders.Where(o => o.OrderID == model.OrderID).FirstOrDefault();
            var data = order.GetUpdateDTO();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public class ShipStatusDTO { public Int32 OrderID { get; set; } public Int32 OrderStatus { get; set; } }
        [HttpPost]
        public HttpStatusCodeResult SetShipping(ShipStatusDTO model){
            repository.UpdateShippingStatus(model.OrderID, model.OrderStatus);
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}