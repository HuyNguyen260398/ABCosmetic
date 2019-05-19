using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;   

namespace ABCosmetic.Models
{
    public class Cart
    {
        ABCosmeticEntities db = new ABCosmeticEntities();
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double ProductPrice { get; set; }
        public int Quantity { get; set; }
        public double Subtotal {
            get { return Quantity * ProductPrice; }
        }
        public Cart(int ProID)
        {
            ProductId = ProID;
            Product product = db.Products.Single(p => p.ID == ProductId);
            ProductName = product.Name;
            ProductImage = product.Image;
            ProductPrice = double.Parse(product.Price.ToString());
            Quantity = 1;
        }
    }
}