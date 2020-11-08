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

        public IQueryable<Product> GetProducts(string q, int page, int pageSize)
        {
            var products = _ctx.Products
                .Where(item => item.Latest)
                .Where(item => item.Name.ToLower().Contains(q.ToLower()));
            return products;
        }

        public async Task<Product> GetProductById(string id, string category)
        {
            return await _ctx.Products
                .OrderByDescending(item => item.Date)
                .FirstOrDefaultAsync(item => item.ProductId == id && item.Category.ToLower().Contains(category));
        }
        public IQueryable<Product> GetProductsById(string id, int page = 1, int pageSize = 10)
        {
            return _ctx.Products
                .Where(item => item.ProductId == id)
                .OrderByDescending(item => item.Date);
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