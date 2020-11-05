using System.Collections.Generic;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetProducts();
        Product GetProductById(string id);
        
        bool SaveChanges();
        
        void CreateProduct(Product product);
    }
}