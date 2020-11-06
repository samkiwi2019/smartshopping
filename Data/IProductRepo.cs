using System.Collections.Generic;
using System.Threading.Tasks;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetProducts(string q, int page, int pageSize);
        Task<Product> GetProductById(string id);

        Task<IEnumerable<Product>> GetProductsById(string id,int page, int pageSize);
        
        Task<bool> SaveChanges();
        
        Task CreateProduct(Product product);
    }
}