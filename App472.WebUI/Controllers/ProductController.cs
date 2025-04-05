using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App472.WebUI.Models;
using System.Configuration;
using App472.WebUI.Domain.Abstract;
using App472.WebUI.Infrastructure;
using System.Net;

namespace App472.WebUI.Controllers
{
    public class ProductController : BaseController
    {
        private IProductsRepository repository;
        public int PageSize = 4;

        public ProductController(IProductsRepository repo)
        {
            this.repository = repo;
        }

        public ActionResult ClearSearch(){
            BaseSessUser sessUser = SessUser;
            sessUser.Search = null;
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        // GET: Product
        // See for security - string search argument
        // https://gosecure.ai/blog/2016/03/22/xss-for-asp-net-developers/
        public ViewResult List(string category, string search, int page = 1)
        {
            // filter search string as it comes in here.
            // filter search string as it comes in here.
            // filter search string as it comes in here.

            BaseSessUser sessUser = SessUser; // get the strongly typed object from session
            if (search != null){
                sessUser.Search = search; // store in session
            }

            ProductsListViewModel model = new ProductsListViewModel{
                Products = repository.InStockProducts
                    .Where(p => category == null || p.Category == category) // if category is null then dont filter by category.
                    .OrderBy(p => p.ID)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo{
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                    repository.InStockProducts.Count() :
                    repository.InStockProducts.Where(e => e.Category == category).Count()
                },
                CurrentCategory = category
            };

            // Get the request url, up to but not including query string.
            var host = Request.Url.AbsoluteUri;
            if (host.Contains("?")){
                host = host.Split('?')[0]; // remove query string
            }
            if (host.EndsWith("/")){
                host = host.Substring(0, host.Length - 1); // remove trailing slash
            }

            // is it secure to do this?
            ViewBag.HostAndPath = host;
            ViewBag.Search = sessUser.Search;

            return View(model);
        }
    }
}