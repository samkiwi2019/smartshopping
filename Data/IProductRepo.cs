using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public interface IProductRepo
    {
        IQueryable<Product> GetProducts(string q, int page, int pageSize);
        
        Task<Product> GetProductById(string id, string category);

        IQueryable<Product> GetProductsById(string id,int page, int pageSize);
        
        Task<bool> SaveChanges();
        
        Task CreateProduct(Product product);
    }
}