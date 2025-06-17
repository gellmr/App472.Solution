using App472.WebUI.Domain.Abstract;
using App472.WebUI.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminProductsController : BaseController
    {
        private IProductsRepository repository;
        private IOrdersRepository ordersRepository;

        public AdminProductsController(IProductsRepository repo, IOrdersRepository orepo)
        {
            repository = repo;
            ordersRepository = orepo;
        }

        public ViewResult Index()
        {
            // Test if we are loading our configuration from the (debug or release) secrets files.
            ViewBag.ConnectionsRelease = ConfigurationManager.ConnectionStrings["ConnectionsRelease"];
            ViewBag.UserSecretsRelease = ConfigurationManager.AppSettings["UserSecretsRelease"];
            ViewBag.SeedJsonRelease = ConfigurationManager.AppSettings["seed.json.release"];

            return View(new AdminProductsViewModel{
                CurrentPageNavText = AppNavs.ProductsNavText,
                Products = repository.InStockProducts
            });
        }

        public ViewResult Edit(int ID, string returnUrl)
        {
            InStockProduct product = repository.InStockProducts.FirstOrDefault(p => p.ID == ID);
            return View(new AdminEditProductViewModel{
                CurrentPageNavText = AppNavs.ProductsNavText,
                Product = product
            });
        }

        [HttpPost]
        public ActionResult Edit(InStockProduct product, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(new AdminEditProductViewModel{
                    CurrentPageNavText = AppNavs.ProductsNavText,
                    Product = product
                });
            }
        }

        public ActionResult Create(string returnUrl)
        {
            return View("Edit", new AdminEditProductViewModel
            {
                CurrentPageNavText = AppNavs.ProductsNavText,
                Product = new InStockProduct
                {
                    Category = "Soccer",
                    Name = "New Product",
                    Description = "Enter your description here",
                    Price = 100
                }
            });
        }

        [HttpPost]
        public ActionResult Delete(int ID)
        {
            if (!ordersRepository.ProductHasOrders(ID)){
                InStockProduct deletedProduct = repository.DeleteProduct(ID);
                if (deletedProduct != null){
                    TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
                }
            }
            else{
                TempData["message"] = "Product exists in one or more orders, it cannot be deleted.";
            }
            return RedirectToAction("Index");
        }
    }
}