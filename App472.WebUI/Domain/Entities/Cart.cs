using System.Collections.Generic;
using System.Linq;

namespace App472.WebUI.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(InStockProduct inStockProduct, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.InStockProduct.ID == inStockProduct.ID)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    InStockProduct = inStockProduct,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(InStockProduct inStockProduct)
        {
            lineCollection.RemoveAll(l => l.InStockProduct.ID == inStockProduct.ID);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.InStockProduct.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine
    {
        public InStockProduct InStockProduct { get; set; }
        public int Quantity { get; set; }
    }
}
