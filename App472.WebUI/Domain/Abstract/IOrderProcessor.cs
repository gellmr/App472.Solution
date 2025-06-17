using App472.WebUI.Domain.Entities;
using System.Web;

namespace App472.WebUI.Domain.Abstract
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails);
    }
}