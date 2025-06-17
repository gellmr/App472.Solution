using App472.WebUI.Domain.Entities;
using System.Web.Mvc;

namespace App472.WebUI.Infrastructure.Binders
{
    public class CartModelBinder:IModelBinder
    {
        private const string sessionKey = "Cart";

        // ---------------------------------------------------------------------------------------
        // This model binder allows us to create Cart objects, using action parameters, like this:
        // public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl){...}
        // ---------------------------------------------------------------------------------------

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            // Get the cart from the session
            Cart cart = null;
            if(controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            // Create the cart if there wasn't one in the session data
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            return cart;
        }
    }
}