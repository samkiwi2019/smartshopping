using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public async Task<IEnumerable<Product>> GetProducts(string q, int page, int pageSize)
        {
            return await _ctx.Products
                .Where(item => item.Latest)
                .Where(item => item.Name.ToLower().Contains(q.ToLower()))
                .Skip((Math.Max(page - 1, 0) * pageSize))
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Product> GetProductById(string id)
        {
            return await _ctx.Products
                .OrderByDescending(item => item.Date)
                .FirstOrDefaultAsync(item => item.ProductId == id);
        }
        
        public async Task<IEnumerable<Product>> GetProductsById(string id, int page = 1, int pageSize = 10)
        {
            return await _ctx.Products
                .Where(item => item.ProductId == id)
                .OrderByDescending(item => item.Date)
                .Skip((Math.Max(page - 1, 0) * pageSize))
                .Take(pageSize)
                .ToListAsync();
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