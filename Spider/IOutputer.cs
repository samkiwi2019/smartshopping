using System.Collections.Generic;
using System.Threading.Tasks;
using Smartshopping.Dtos;

namespace Smartshopping.Spider
{
    public interface IOutputer
    {
        Task CollectData(IList<ProductCreateDto> products);
    }
}