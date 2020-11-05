using System.Collections.Generic;
using Smartshopping.Dtos;

namespace Smartshopping.Spider
{
    public class ProductParse
    {
        public IEnumerable<ProductCreateDto> Products { get; set; }
        public IEnumerable<string> Urls { get; set; }
    }
}