using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartshopping.Dtos;

namespace Smartshopping.Data
{
    public class PaginationBase<T>
    {
        public int Page { get; set; } 
        public int PageSize { get; set; } 
        public int Total { get; set; } 
        
        public PaginationBase(IQueryable<T> list, int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize < 1 ? 10 : pageSize;
            Total = list.Count();
        }
    }
}