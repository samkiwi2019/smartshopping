using System;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using FluentScheduler;
using Microsoft.AspNetCore.Mvc;
using Smartshopping.Data;
using Smartshopping.Dtos;

namespace Smartshopping.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProductReadDto>> GetProducts()
        {
            var productItems = _repository.GetProducts();

            JobManager.AddJob(() =>
            {
                Console.WriteLine("I am running!");
                Spider.SpiderMaker.Crawl();
                
            }, (s) => s.ToRunNow());
                // .ToRunEvery(1).Days().At(3, 0)
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }
    }
}