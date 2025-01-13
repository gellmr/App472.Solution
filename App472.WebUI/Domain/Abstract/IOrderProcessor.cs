using App472.WebUI.Domain.Entities;
using System.Web;

namespace App472.WebUI.Domain.Abstract
{
    // To allow for different implementations eg Email, App, etc
    // And to allow for mocking so we can unit test our controller without sending emails.
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}