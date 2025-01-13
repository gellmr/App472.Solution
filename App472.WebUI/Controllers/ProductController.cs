using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App472.WebUI.Models;
using System.Configuration;
using App472.WebUI.Domain.Abstract;

namespace App472.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductsRepository repository;
        public int PageSize = 4;

        public ProductController(IProductsRepository repo)
        {
            this.repository = repo;
        }

        // GET: Product
        public ViewResult List(string category, int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel{
                Products = repository.Products
                    .Where(p => category == null || p.Category == category) // if category is null then dont filter by category.
                    .OrderBy(p => p.ProductID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo{
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    repository.Products.Count() :
                    repository.Products.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }
    }
}