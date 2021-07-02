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
    public abstract class CommonRepo<T> : ICommonRepo<T> where T : BaseEntity
    {
        protected readonly MyContext Ctx;
        protected readonly DbSet<T> DbSet;

        protected CommonRepo(MyContext context)
        {
            Ctx = context;
            DbSet = context.Set<T>();
        }

        public virtual async Task Create(T t)
        {
            if (t == null)
            {
                throw new ArgumentException(nameof(t));
            }

            await DbSet.AddAsync(t);
        }

        public virtual async Task<bool> Delete(int id)
        {
            var result = await DbSet.FindAsync(id);
            Ctx.Remove(result);
            return await Ctx.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> SaveChange()
        {
            return await Ctx.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> Update(T t)
        {
            Ctx.Update(t);
            return await Ctx.SaveChangesAsync() > 0;
        }

        public virtual PagedResult<T> GetPagedItems(SearchParams searchParams)
        {
            var items = Search(searchParams);
            return new PagedResult<T>(items, searchParams.CurrPage, searchParams.PageSize);
        }

        public virtual async Task<T> GetItemById(int id)
        {
            return await DbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IQueryable<T> Search(SearchParams searchParams)
        {
            return DbSet
                .Where(product => product.Latest)
                .WhereIf(searchParams.IsPromotion, product => !string.IsNullOrEmpty(product.Prefix))
                .WhereIf(!string.IsNullOrEmpty(searchParams.Category),
                    product => product.Category.Contains(searchParams.Category, StringComparison.OrdinalIgnoreCase))
                .WhereIf(!string.IsNullOrEmpty(searchParams.Query),
                    product => product.Name.Contains(searchParams.Query, StringComparison.OrdinalIgnoreCase))
                .OrderByIf(searchParams.SortBy, false);
        }
    }
}