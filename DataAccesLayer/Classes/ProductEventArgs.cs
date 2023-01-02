using System;

namespace DataAccesLayer.Classes
{
    public class ProductEventArgs : EventArgs
    {
        public ProductModel.ProductModel Product { private set; get; }
        public ProductEventArgs(ProductModel.ProductModel product)
        {
            Product = product;
        }
    }
}
