using Microsoft.EntityFrameworkCore;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public class MyContext: DbContext
    {
        public MyContext(DbContextOptions<MyContext> opt) : base(opt)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}