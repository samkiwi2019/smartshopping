using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public class SqlProductRepo : IProductRepo
    {
        private readonly MyContext _ctx;

        public SqlProductRepo(MyContext ctx)
        {
            _ctx = ctx;
        }

        public IQueryable<Product> GetProducts(string q, int page, int pageSize, string category, bool isPromotion)
        {
            var products = _ctx.Products
                .Where(item =>
                    item.Latest 
                    && (!isPromotion || item.Prefix.Length > 0) 
                    && item.Category.ToLower().Contains(category.ToLower())
                    && item.Name.ToLower().Contains(q.ToLower())
                    )
                .OrderByDescending(item => item.Date)
                .ThenBy(item => item.Compare);
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

        public async Task<IList<Product>> GetProductsById(string id, int page = 1, int pageSize = 10)
        {
            return await _ctx.Products
                .Where(item => item.ProductId == id)
                .Skip(Math.Max(page - 1, 0) * pageSize).Take(pageSize).ToListAsync();
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
    }
}