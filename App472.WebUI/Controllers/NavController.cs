using App472.WebUI.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App472.WebUI.Controllers
{
    public class NavController : Controller
    {
        private IProductsRepository repository;
        public NavController(IProductsRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult CategoryMenu(string category = null){
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = repository.InStockProducts
                .Select(x => x.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
    }
}