using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Smartshopping.Cache;
using Smartshopping.Data;
using Smartshopping.Data.IRepos;
using Smartshopping.Dtos;
using Smartshopping.Library;
using Smartshopping.Models;

namespace Smartshopping.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class ProductController: CommonController
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepo productRepo, IMapper mapper)
        {
            _repository = productRepo;
            _mapper = mapper;
        }
        
        [HttpGet("products")]
        [Cached(600)]
        public IActionResult GetItems([FromQuery] SearchParams searchParams)
        {
            try
            {
                var pagedResult = _repository.GetPagedItems(searchParams);
                var items = _mapper.Map<IList<ProductReadDto>>(pagedResult.Items);
                var pagination = pagedResult.Pagination;
                return Ok(new {items, pagination});
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }
        
        [HttpGet("products/{productId}/items")]
        [Cached(600)]
        public async Task<IActionResult> GetItemsById(string productId)
        {
            try
            {
                var productItem = await _repository.GetProductsById(productId);
                var items = _mapper.Map<IList<ProductReadDto>>(productItem);
                return Ok(new {items});
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }
        
        // GET /api/products/{productId}
        [HttpGet("{productId}")]
        [Cached(600)]
        public async Task<ActionResult<ProductReadDto>> GetProductByProductId(string productId, string category = "")
        {
            try
            {
                var productItem = await _repository.GetProductById(productId, category);
                return Ok(_mapper.Map<ProductReadDto>(productItem));
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }

        // GET /api/products/related
        [HttpGet("related")]
        [Cached(600)]
        public async Task<ActionResult<IList<ProductReadDto>>> GetProductsByRelated(string name, string category = "")
        {
            try
            {
                var query = await _repository.GetProductByRelated(name.Split(" ").Last(), category);
                var items = _mapper.Map<IList<ProductReadDto>>(query);
                return Ok(items);
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }


        // GET /api/products/setSpiderSchedule
        [HttpPost("setSpiderSchedule")]
        public ActionResult SetSpiderSchedule()
        {
            Spider.SpiderMaker.GetAJob();
            return Ok("The Spider has already got a job!");
        }
    }
}