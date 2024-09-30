using App472.Domain.Abstract;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace App472.WebUI.Controllers
{
    public class AdminUserOrderController : Controller
    {
        private IOrdersRepository repository;

        public AdminUserOrderController(IOrdersRepository repo){
            this.repository = repo;
        }

        public ActionResult Index(int Id = 1)
        {
            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel{
                UserId = Id,
                Orders = repository.Orders.Where(o => o.UserID == Id)
                .OrderBy(o => o.OrderID)
            };
            return View(model);
        }
    }
}