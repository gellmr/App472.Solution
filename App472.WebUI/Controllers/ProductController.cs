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
    public ActionResult List(string category, string search = null, bool clear = false, int page = 1)
    {
      if (clear)
      {
        // Clear the search string
        SessUser.Search = null; 
        search = null;
      }
      else if (string.IsNullOrEmpty(search))
      {
        // User did not provide a search string
        search = SessUser.Search; // Retrieve from session. Already validated.
      }
      else
      {
        // User did provide a search string. Validate the search string...
        // Search string: only allow alphanumeric, space, dash, up to 40 characters
        if (!MyExtensions.ValidateString(search, "^[A-Za-z0-9\\s\\-]{0,40}$"))
        {
          SessUser.Search = null; // Clear the search string from session
          return new HttpStatusCodeResult(HttpStatusCode.BadRequest); // 400
        }
        // Because of the very restrictive regex above, we wont get this far if there are any dangerous characters in the search string.
        // Persist the search string across requests...
        SessUser.Search = search; // Store in session
      }

      // Done validating our search string.
      // Convert for use in linq query...
      string searchlow = string.IsNullOrEmpty(search) ? search : search.ToLower();

      // Query the database for products matching our search string, and chosen category if enabled.
      // If no search string and no category, it will find all products.
      IEnumerable<InStockProduct> unpaginated = repository.InStockProducts.Where(p => (
        // True if we are not filtering by category, or if the product is from the category we want
        category == null || p.Category == category
      ) && (
        // True if our search string is empty, or the product details contain our search string.
        string.IsNullOrEmpty(searchlow)
        || (p.Name).ToLower().Contains(searchlow)
        || (p.Description).ToLower().Contains(searchlow)
        || (p.Category).ToLower().Contains(searchlow)
      )).OrderBy(p => p.ID);

      int unpagTot = unpaginated.Count();  // eg 11
      int skipN = ((page - 1) * PageSize); // eg 8
      if (unpagTot <= skipN) // eg if the asked for page 4  (11 <= 12)
      {
        // User is requesting page greater than our maximum pagination
        int numPages = (int)Math.Floor((decimal)unpagTot / (decimal)PageSize); // eg (11 / 4) == 2.75  Floor: 2
        int mod = unpagTot % PageSize; // eg 11 mod 4 == 3
        if (mod == 0)
        {
          page = numPages; // We are on page 2
        }
        else
        {
          page = numPages + 1; // We are on page 3
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
      if (host.Contains("?"))
      {
        host = host.Split('?')[0]; // Remove query string
      }
      if (host.EndsWith("/"))
      {
        host = host.Substring(0, host.Length - 1); // Remove trailing slash
      }

      ProductsListViewModel model = new ProductsListViewModel
      {
        Products = results,
        PagingInfo = paging,
        CurrentCategory = category,
        SearchString = HttpUtility.HtmlEncode(search), // To display in search text box
        HostAndPath = host, // To construct url for SearchProducts function in javascript
        RenderFragment = this.IsJsonRequest // True if we only want to render part of the page.
      };
      return View(model);
    }
  }
}