using System;
using System.Linq;
using System.Web.Mvc;
using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.WebUI.Infrastructure;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;

namespace App472.WebUI.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private IProductsRepository repository;

        public AdminController(IProductsRepository repo){
            repository = repo;
        }

        public ViewResult Index(string returnUrl)
        {
            string url = string.IsNullOrEmpty(returnUrl) ? GenerateTabReturnUrl.ToString() : GetTabReturnUrl(returnUrl);
            return View(new AdminProductsViewModel{
                CurrentPageNavText = AppNavs.ProductsNavText,
                Products = repository.Products,
                ReturnUrl = url
            });
        }

        public ViewResult Edit(int productId, string returnUrl)
        {
            string url = GetTabReturnUrl(returnUrl);
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(new AdminEditProductViewModel{
                CurrentPageNavText = AppNavs.ProductsNavText,
                Product = product,
                ReturnUrl = url
            });
        }

        [HttpPost]
        public ActionResult Edit(Product product, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string url = GetTabReturnUrl(returnUrl);
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                //return RedirectToAction("Index");
                return Redirect(url ?? Url.Action("Index", "Admin"));
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
            string url = GetTabReturnUrl(returnUrl);
            return View("Edit", new AdminEditProductViewModel{
                CurrentPageNavText = AppNavs.ProductsNavText,
                Product = new Product{
                    Category = "Soccer",
                    Name = "Product Name",
                    Description = "Amazing New Product",
                    Price = 100
                },
                ReturnUrl = url
            });
        }

        [HttpPost]
        public ActionResult Delete(int productId){
            Product deletedProduct = repository.DeleteProduct(productId);
            if(deletedProduct != null){
                TempData["message"] = string.Format("{0} was deleted", deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}