using App472.Domain.Abstract;
using App472.Domain.Entities;
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
    public class AdminUserOrderController : Controller
    {
        private IOrdersRepository repository;

        public AdminUserOrderController(IOrdersRepository repo)
        {
            this.repository = repo;
        }

        public ActionResult Index(int UserId = 1)
        {
            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel
            {
                LinkText = "Edit Users",
                UserId = UserId,
                Orders = repository.Orders.Where(o => o.UserID == UserId)
                .OrderBy(o => o.OrderID)
            };
            return View(model);
        }

        public ActionResult Detail(Int32 OrderID)
        {
            Order order = repository.Orders.Where(o => o.OrderID == OrderID).FirstOrDefault();
            IEnumerable<OrderedProduct> orderedProducts = order.OrderedProducts;
            if (order.OrderStatus == null){order.OrderStatus = Order.ParseShippingState(ShippingState.NotYetPlaced);}
            AdminUserOrderDetailViewModel model = new AdminUserOrderDetailViewModel{
                LinkText = "Edit Users",
                UserId = (Int32)order.UserID,
                OrderID = OrderID,
                OrderedProducts = orderedProducts.ToList(),
                OrderPlacedDate = order.OrderPlacedDate,
                PaymentReceivedDate = order.PaymentReceivedDate,
                ReadyToShipDate = order.ReadyToShipDate,
                ShipDate = order.ShipDate,
                ReceivedDate = order.ReceivedDate,
                BillingAddress = order.BillingAddress,
                ShippingAddress = order.ShippingAddress,
                OrderStatus = App472.Domain.Entities.Order.ParseShippingState(order.OrderStatus)
            };
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
            Order order = repository.Orders.Where(o => o.OrderID == model.OrderID).FirstOrDefault();
            var data = order.GetUpdateDTO();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}