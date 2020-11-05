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
        private MyContext _myContext;
        private IMapper _mapper;

        public Outputer()
        {
            _myContext = GlobalAppConst.ServiceProvider.GetService<MyContext>();
            _mapper = GlobalAppConst.ServiceProvider.GetService<IMapper>();
        }

        public void CollectData(List<ProductCreateDto> products)
        {
            foreach (var product in products)
            {
                var oldProduct = _myContext.Products.OrderByDescending(item => item.Date)
                    .FirstOrDefault(item => item.ProductId == product.ProductId);
                if (oldProduct != null)
                {
                    oldProduct.Latest = false;
                    product.Compare = (Convert.ToDouble(product.Price) - Convert.ToDouble(oldProduct.Price)) /
                                      Convert.ToDouble(oldProduct.Price);
                }

                var productModal = _mapper.Map<Product>(product);
                _myContext.Products.Add(productModal);
            }
            _myContext.SaveChanges();
        }
    }
}