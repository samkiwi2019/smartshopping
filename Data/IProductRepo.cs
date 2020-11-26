using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetProducts(string q, int page, int pageSize, string category, bool isPromotion);
        
        Task<Product> GetProductById(string id, string category);
        
        Task<IList<Product>> GetProductByRelated(string name, string category);

        Task<IList<Product>> GetProductsById(string id);
        
        Task<bool> SaveChanges();
        
        Task CreateProduct(Product product);
    }
}