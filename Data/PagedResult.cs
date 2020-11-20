using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartshopping.Models;

namespace Smartshopping.Data
{
    public class PagedResult<T>
    {
        public PaginationBase<T> Pagination { get; set; }
        public IList<T> Items { get; set; }

        public PagedResult(IEnumerable<T> list, int page, int pageSize)
        {
            Pagination = new PaginationBase<T>(list, page, pageSize);
            Items = list.Skip(Math.Max(page - 1, 0) * pageSize).Take(pageSize).ToList();
        }
    }
}