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
        private readonly MyContext _context;
        private readonly DbSet<T> _dbSet;
        
        protected CommonRepo(MyContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        
        public virtual Task<T> Create(T t)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<T> Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public virtual Task<T> Update(T t)
        {
            throw new System.NotImplementedException();
        }

        public virtual PagedResult<T> GetPagedItems(SearchParams searchParams)
        {
            var items = Search(searchParams);
            return new PagedResult<T>(items, searchParams.CurrPage, searchParams.PageSize);
        }

        public virtual async Task<T> GetItemById(int id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual IQueryable<T> Search(SearchParams searchParams)
        {
            return _dbSet
                .Where(product => product.Latest)
                .WhereIf(!string.IsNullOrEmpty(searchParams.Q), product => product.Name.Contains(searchParams.Q))
                .MultipleOrderByIf(searchParams.SortBy != null, searchParams.SortBy);
        }
    }
}