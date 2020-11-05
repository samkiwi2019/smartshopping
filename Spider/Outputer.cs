using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Smartshopping.Data;
using Smartshopping.Dtos;
using Smartshopping.Models;

namespace Smartshopping.Spider
{
    public class Outputer
    {
        private IProductRepo _repository;
        private IMapper _mapper;

        public Outputer()
        {
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();
            _repository = provider.CreateScope().ServiceProvider.GetService<SqlProductRepo>();
            _mapper = provider.GetService<IMapper>();
        }

        public void CollectData(List<ProductCreateDto> products)
        {
            

            // foreach (var product in products)
            // {
            //     Console.WriteLine(_repository);
            //     var oldProduct = _repository.GetProductById(product.ProductId);
            //     Console.WriteLine("数据{0}", oldProduct);
            //     if (oldProduct != null)
            //     {
            //         oldProduct.Latest = false;
            //         product.Compare = (Convert.ToDouble(product.Price) - Convert.ToDouble(oldProduct.Price)) /
            //                           Convert.ToDouble(oldProduct.Price);
            //     }
            //     
            //     var productModel = _mapper.Map<Product>(product);
            //     _repository.CreateProduct(productModel);
            // }

            // _repository.SaveChanges();
        }
    }
}