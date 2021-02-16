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
    public class PakProductRepo: CommonRepo<Product>, IPakProductRepo
    {
        private readonly DbSet<Product> _dbSet;
        private readonly MyContext _ctx;
        public PakProductRepo(MyContext context) : base(context)
        {
            _dbSet = context.Set<Product>();
            _ctx = context;
        }

          public IEnumerable<Product> GetProducts(string q, int page, int pageSize, string category, bool isPromotion)
        {
            var products = _ctx.Products.Where(item => item.Latest)
                .Where(item => 
                    item.Name.ToLower().Contains(q.ToLower())
                    && item.Category.ToLower().Contains(category.ToLower())
                    && (!isPromotion || item.Prefix.Length > 0))
                .OrderBy(item => item.Compare);
            
            return products;
        }

        public async Task<Product> GetProductById(string id, string category)
        {
            return await _ctx.Products
                .OrderByDescending(item => item.Date)
                .FirstOrDefaultAsync(item => item.ProductId == id && item.Category.ToLower().Contains(category));
        }

        public async Task<IList<Product>> GetProductByRelated(string name, string category)
        {
            return await _ctx.Products
                .Where(item => item.Name.ToLower().Contains(name.ToLower()))
                .Where(item => item.Latest)
                .OrderBy(item => Convert.ToDecimal(item.Price))
                .ToListAsync();
        }

        public async Task<IList<Product>> GetProductsById(string id)
        {
            return await _ctx.Products
                .Where(item => item.ProductId == id)
                .OrderByDescending(item => item.Date)
                .Take(100).ToListAsync();
        }

        public async Task<bool> SaveChanges()
        {
            return await _ctx.SaveChangesAsync() > 0;
        }

        public async Task CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }

            await _ctx.Products.AddAsync(product);
        }

        public async Task<bool> MarkProductsToHistory()
        {
             _ctx.Products.Where(item => item.Latest).SetValue(item => item.Latest = false);
             return await _ctx.SaveChangesAsync() > 0;
        }

        public decimal GetAvgPriceById(string id)
        {
            var res = _ctx.Products
                .Where(item => item.ProductId == id)
                .OrderByDescending(item => item.Date)
                .Take(30)
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