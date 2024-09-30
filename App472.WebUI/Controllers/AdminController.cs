using System.Linq;
using System.Web.Mvc;
using App472.Domain.Abstract;
using App472.Domain.Entities;
using App472.WebUI.Models;
using Microsoft.AspNet.Identity;

namespace App472.WebUI.Controllers{

    [Authorize]
    public class AdminController : Controller{
        private IProductsRepository repository;

        public AdminController(IProductsRepository repo){
            repository = repo;
        }

        public ViewResult Index(){
            return View(new AdminProductsViewModel{
                LinkText = "Edit Products",
                Products = repository.Products
            });
        }

        public ViewResult Edit(int productId){
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            return View(new AdminEditProductViewModel{
                LinkText = "Edit Products",
                Product = product
            });
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Create(){
            return View("Edit", new Product());
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