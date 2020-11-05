using AutoMapper;
using Smartshopping.Dtos;
using Smartshopping.Models;

namespace Smartshopping.Profiles
{
    public class ProductProfiles : Profile
    {
        public ProductProfiles()
        {
            // source => target
            CreateMap<Product, ProductReadDto>();
            CreateMap<ProductCreateDto, Product>();
        }
    }
}