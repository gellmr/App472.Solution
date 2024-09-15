using App472.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App472.Domain.Entities;
using App472.WebUI.Models;

namespace App472.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductsRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductsRepository repo, IOrderProcessor proc){
            repository = repo;
            orderProcessor = proc;
        }
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl){
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if ( product != null) {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl){
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if(product != null){
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        
        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel{
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        public ViewResult Checkout(){
            return View(new App472.Domain.Entities.ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails){
            if(cart.Lines.Count() == 0){
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }
            if (ModelState.IsValid){
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }else{
                return View(shippingDetails);
            }
        }
    }
}