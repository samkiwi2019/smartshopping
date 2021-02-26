using System.Linq;
using System.Threading.Tasks;

namespace Smartshopping.Data.IRepos
{
    public interface ICommonRepo<T>
    {
        Task<bool> Create(T t);
        Task<bool> Delete(int id);
        Task<bool> Update(T t);
        Task<bool> SaveChange();
        PagedResult<T> GetPagedItems(SearchParams searchParams);
        Task<T> GetItemById(int id);
        IQueryable<T> Search(SearchParams searchParams);
    }
}