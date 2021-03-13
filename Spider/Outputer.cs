using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Smartshopping.Data.IRepos;
using Smartshopping.Dtos;
using Smartshopping.Library;
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

            await productModals.ForEachAsync(async product =>
            {
                var avg = await _repository.GetAvgPriceById(product.ProductId);
                var diff = avg == -999 ? 1 : (Convert.ToDecimal(product.Price) - avg) / avg;
                product.Compare = Convert.ToDouble(diff);
                await _repository.Create(product);
            });
            await _repository.SaveChange();
        }
    }
}