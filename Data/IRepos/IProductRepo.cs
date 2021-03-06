using System.Collections.Generic;
using System.Threading.Tasks;
using Smartshopping.Models;

namespace Smartshopping.Data.IRepos
{
    public interface IProductRepo: ICommonRepo<Product>
    {
        Task<Product> GetProductById(string id, string category);
        
        Task<IList<Product>> GetProductByRelated(string name, string category);

        Task<IList<Product>> GetProductsById(string id);
        
        Task<bool> MarkProductsToHistory();

        decimal GetAvgPriceById(string id);
    }
}