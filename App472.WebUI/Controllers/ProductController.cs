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
using App472.WebUI.Domain.Entities;

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

        // GET: Product
        // See for security - string search argument
        // https://gosecure.ai/blog/2016/03/22/xss-for-asp-net-developers/
        public ActionResult List(string category, string search = null, bool clear = false, int page = 1)
        {
            // ----------------------------------------------------------------
            // See https://stackoverflow.com/questions/2200788/asp-net-request-validation-causes-is-there-a-list
            // See https://stackoverflow.com/questions/74218952/query-string-parameter-value-truncate-after
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
            // In the above example I submitted html encoded query string directly in the request.
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

            if (clear) // Does the user want to clear the search string?
            {
                // clear the search string...
                SessUser.Search = null;
                search = null;

            }
            else if (string.IsNullOrEmpty(search)) // Did the user provide a search string?
            {
                // no
                search = SessUser.Search; // retrieve from session. Already validated.
            }
            else
            {
                // yes
                // Validate the search string...

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

                // Because of the very restrictive regex above, we wont get this far
                // if there are any dangerous characters in the search string.

                // Not needed. Dashes and spaces dont get html encoded.
                // search = HttpUtility.HtmlEncode(search);

                // Persist the search string across requests...
                SessUser.Search = search; // store in session
            }

            // Done validating our search string.
            // Convert for use in linq query...
            string searchlow = string.IsNullOrEmpty(search) ? search : search.ToLower();

            // Query the database for products matching our search string, and chosen category if enabled.
            // If no search string and no category, it will find all products.
            IEnumerable<InStockProduct> unpaginated = repository.InStockProducts.Where(p =>
                (
                    // true if we are not filtering by category, or if the product is from the category we want
                    category == null || p.Category == category
                )
                &&
                (
                    // true if our search string is empty, or the product details contain our search string.
                    string.IsNullOrEmpty(searchlow)
                    || (p.Name).ToLower().Contains(searchlow)
                    || (p.Description).ToLower().Contains(searchlow)
                    || (p.Category).ToLower().Contains(searchlow)
                )
            )
            .OrderBy(p => p.ID);

            int unpagTot = unpaginated.Count();  // eg 11
            int skipN = ((page - 1) * PageSize); // eg 8
            if (unpagTot <= skipN) // eg if the asked for page 4  (11 <= 12)
            {
                // user is requesting page greater than our maximum pagination
                int numPages = (int)Math.Floor( (decimal)unpagTot / (decimal)PageSize ); // eg (11 / 4) == 2.75  Floor: 2
                int mod = unpagTot % PageSize; // eg 11 mod 4 == 3
                if (mod == 0){
                    page = numPages; // we are on page 2
                }else{
                    page = numPages + 1; // we are on page 3
                }
            }

            IEnumerable<InStockProduct> results = unpaginated
            .Skip((page - 1) * PageSize) //  1,2,3,4,    5,6,7,8,    9,10,11
            //    (skip 3-1) * 4             x x x x     x x x x     ^page=3
            .Take(PageSize);//                                       (take 4)

            PagingInfo paging = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = unpaginated.Count()
            };

            // Get the request url, up to but not including query string.
            var host = Request.Url.AbsoluteUri;
            if (host.Contains("?")){
                host = host.Split('?')[0]; // remove query string
            }
            if (host.EndsWith("/")){
                host = host.Substring(0, host.Length - 1); // remove trailing slash
            }

            ProductsListViewModel model = new ProductsListViewModel
            {
                Products = results,
                PagingInfo = paging,
                CurrentCategory = category,

                SearchString = HttpUtility.HtmlEncode(search), // to display in search text box
                HostAndPath = host, // to construct url for SearchProducts function in javascript
                RenderFragment = this.IsJsonRequest // true if we only want to render part of the page.
            };
            
            return View(model);
        }
    }
}