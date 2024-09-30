﻿using App472.Domain.Abstract;
using App472.Domain.Concrete;
using App472.Domain.Entities;
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
        private IOrderedProductsRepository opRepository;

        public AdminUserOrderController(IOrdersRepository repo, IOrderedProductsRepository opRepo)
        {
            this.repository = repo;
            this.opRepository = opRepo;
        }

        public ActionResult Index(int Id = 1)
        {
            AdminUserOrdersViewModel model = new AdminUserOrdersViewModel
            {
                LinkText = "Edit Users",
                UserId = Id,
                Orders = repository.Orders.Where(o => o.UserID == Id)
                .OrderBy(o => o.OrderID)
            };
            return View(model);
        }


        public ActionResult Detail(Int32 OrderID)
        {
            Order order = repository.Orders.Where(o => o.OrderID == OrderID).FirstOrDefault();
            AdminUserOrderDetailViewModel model = new AdminUserOrderDetailViewModel
            {
                LinkText = "Edit Users",
                UserId = (Int32)order.UserID,
                OrderID = OrderID,
                OrderedProducts = opRepository.OrderedProducts.Where(op => op.Order.OrderID == OrderID)
            };
            return View(model);
        }
    }
}