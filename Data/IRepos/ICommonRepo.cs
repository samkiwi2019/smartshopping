using System.Linq;
using System.Threading.Tasks;

namespace Smartshopping.Data.IRepos
{
    public interface ICommonRepo<T>
    {
        Task<T> Create(T t);
        Task<T> Delete(int id);
        Task<T> Update(T t);
        PagedResult<T> GetPagedItems(SearchParams searchParams);
        Task<T> GetItemById(int id);
        IQueryable<T> Search(SearchParams searchParams);
    }
}