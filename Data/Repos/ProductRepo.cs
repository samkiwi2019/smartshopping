using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smartshopping.Data.IRepos;
using Smartshopping.Library;
using Smartshopping.Models;

namespace Smartshopping.Data.Repos
{
    public class ProductRepo: CommonRepo<Product>, IProductRepo
    {
        public ProductRepo(MyContext context) : base(context)
        {
        }
        
        public async Task<Product> GetProductById(string id, string category)
        {
            return await DbSet
                .OrderByDescending(item => item.Date)
                .FirstOrDefaultAsync(item => item.ProductId == id && item.Category.ToLower().Contains(category));
        }

        public async Task<IList<Product>> GetProductByRelated(string name, string category)
        {
            return await DbSet
                .Where(item => item.Name.ToLower().Contains(name.ToLower()))
                .Where(item => item.Latest)
                .OrderBy(item => Convert.ToDecimal(item.Price))
                .ToListAsync();
        }

        public async Task<IList<Product>> GetProductsById(string productId)
        {
            return await DbSet
                .Where(item => item.ProductId == productId)
                .OrderByDescending(item => item.Date)
                .Take(30).ToListAsync();
        }

        public async Task<bool> MarkProductsToHistory()
        {
             DbSet.Where(item => item.Latest).SetValue(item => item.Latest = false);
             return await Ctx.SaveChangesAsync() > 0;
        }

        public decimal GetAvgPriceById(string id)
        {
            // compare curr product over past 90 days.
            var res = DbSet
                .Where(item => item.ProductId == id)
                .OrderByDescending(item => item.Date)
                .Take(90)
                .AsEnumerable()
                .GroupBy(
                    item => item.Latest.ToString(),
                    item => Convert.ToDecimal(item.Price), 
                    (key, products) => new
                    {
                        avg = products.AsQueryable().Average()
                    }
                ).FirstOrDefault();

            return res?.avg ?? -999;
        }
    }
}