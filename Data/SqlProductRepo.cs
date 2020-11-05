using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public IEnumerable<Product> GetProducts()
        {
            return _ctx.Products.Take(10).ToList();
        }

        public Product GetProductById(string id)
        {
            return _ctx.Products.OrderByDescending(item=> item.Date).FirstOrDefault(item => item.ProductId == id);
        }

        public bool SaveChanges()
        {
            return _ctx.SaveChanges() >= 0;
        }

        public void CreateProduct(Product product)
        {
            if (product == null)
            {
                throw new ArgumentException(nameof(product));
            }
            _ctx.Products.Add(product);
        }
    }
}