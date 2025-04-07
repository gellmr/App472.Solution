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
using PCRE;

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
        public ActionResult List(string category, string search, int page = 1)
        {
            // ----------------------------------------------------------------
            // See https://stackoverflow.com/questions/2200788/asp-net-request-validation-causes-is-there-a-list
            // Note that .NET Framework is automatically truncating search if it finds & ; <
            //  ...it will detect search=&lt;script&gt;alert(&#39;XSS&#39;);&lt;/script&gt;
            //                           ^         ^         ^^      ^^     ^          ^
            //                           Any of these characters will result in the framework silently stripping the
            //                           search string, after the offending character.
            //     So if you put search=t;alert(&#39;XSS&#39;);&lt;/script&gt;
            //                                  ^
            // It will result in search=t;alert(
            //
            // Not sure how it handles legitimate &name=mike within a query string
            // or if this truncating behaviour is happening before or after encoding.
            // or if it applies when i submit encoded input like &quot;
            //
            // It will also silently convert   aaa%aaa   into   aaa�a
            //                                                     ^%aa becomes �
            // Note that aaa^aaa is allowed
            // Note that aaa+aaa becomes aaa aaa
            // Note that aaa\aaa becomes aaa\\aaa
            // Note that aaa/aaa is allowed
            // Note that aaa<aaa does not make it past the request validation. You see error message in browser.
            // Note that aaa>aaa is allowed
            // Note that aaa?aaa is allowed
            // I think .NET examining the request, to see if it contains a dangerous string.
            // ----------------------------------------------------------------

            // Search string: only allow alphanumeric, space, dash, up to 40 characters
            if (!string.IsNullOrEmpty(search))
            {
                const string validationPattern = "^[A-Za-z0-9\\s\\-]{0,40}$"; // very restrictive regex
                var regex = new PcreRegex(validationPattern);
                bool isValidSearchString = regex.IsMatch(search);
                if (!isValidSearchString)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
                }
            }

            // Because of the very restrictive regex above, we probably wont get this far
            // if there are any dangerous characters in the search string.

            search = HttpUtility.HtmlEncode(search); // ensure input is html encoded.

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
            ViewBag.IsFragment = this.IsJsonRequest;
            
            return View(model);
        }
    }
}