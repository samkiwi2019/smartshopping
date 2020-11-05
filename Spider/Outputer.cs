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

        public void CollectData(IEnumerable<ProductCreateDto> products)
        {
            foreach (var product in products)
            {
                var oldProduct = _repository.GetProductById(product.ProductId);
                if (oldProduct != null)
                {
                    oldProduct.Latest = false;
                    product.Compare = (Convert.ToDouble(product.Price) - Convert.ToDouble(oldProduct.Price)) /
                                      Convert.ToDouble(oldProduct.Price);
                }

                var productModel = _mapper.Map<Product>(product);
                _repository.CreateProduct(productModel);
            }

            _repository.SaveChanges();
        }
    }
}