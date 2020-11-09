using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Smartshopping.Data;
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
                var oldProduct = await _repository.GetProductById(product.ProductId, product.Category);
                if (oldProduct != null)
                {
                    Console.WriteLine(oldProduct.ProductId);
                    oldProduct.Latest = false;
                    product.Compare =
                        product.Price == oldProduct.Price
                            ? 1
                            : (Convert.ToDouble(product.Price) - Convert.ToDouble(oldProduct.Price)) /
                              Convert.ToDouble(oldProduct.Price);
                }

                await _repository.CreateProduct(product);
            }

            await _repository.SaveChanges();
        }
    }
}