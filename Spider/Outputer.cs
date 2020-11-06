using System;
using System.Collections.Generic;
using AutoMapper;
using Smartshopping.Data;
using Smartshopping.Dtos;
using Smartshopping.Models;

namespace Smartshopping.Spider
{
    public class Outputer
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public Outputer(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async void CollectData(IEnumerable<ProductCreateDto> products)
        {
            foreach (var product in products)
            {
                var oldProduct = await _repository.GetProductById(product.ProductId);
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

                var productModal = _mapper.Map<Product>(product);
                await _repository.CreateProduct(productModal);
            }

            await _repository.SaveChanges();
        }
    }
}