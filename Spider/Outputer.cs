using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Smartshopping.Data;
using Smartshopping.Data.IRepos;
using Smartshopping.Dtos;
using Smartshopping.Models;

namespace Smartshopping.Spider
{
    public class Outputer: IOutputer
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public Outputer(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task CollectData(IList<ProductCreateDto> products)
        {
            var productModals = _mapper.Map<IList<Product>>(products);
            foreach (var product in productModals)
            {
                var avg = _repository.GetAvgPriceById(product.ProductId);
                var diff = avg == -999 ? 1 : (Convert.ToDecimal(product.Price) - avg) / avg;
                product.Compare = Convert.ToDouble(diff);
                await _repository.CreateProduct(product);
            }
            await _repository.SaveChanges();
        }
    }
}