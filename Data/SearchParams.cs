using System.Collections.Generic;

namespace Smartshopping.Data
{
    public class SearchParams
    {
        public string Q { get; set; } = "";
        public int CurrPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Category { get; set; } = "";
        public bool IsPromotion { get; set; } = false;
        public IDictionary<string, int> SortBy { get; set; }
    }
}