using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper.Internal;
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

        public IEnumerable<Product> GetProducts(string q, int page, int pageSize, string category, bool isPromotion)
        {
            var products = _ctx.Products.Where(item => item.Latest)
                .Where(item => 
                    item.Name.ToLower().Contains(q.ToLower())
                    && item.Category.ToLower().Contains(category.ToLower())
                    && (!isPromotion || item.Prefix.Length > 0))
                .AsEnumerable()
                .Where(item => item.Date.Subtract(DateTime.Now).Days == 0)
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
    }
}