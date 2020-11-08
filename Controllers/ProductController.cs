using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentScheduler;
using Microsoft.AspNetCore.Mvc;
using Smartshopping.Data;
using Smartshopping.Dtos;
using Smartshopping.Library;
using Smartshopping.Models;

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

        // GET /api/products
        [HttpGet]
        public ActionResult GetProducts(string q = "", int page = 1, int pageSize = 10)
        {
            try
            {
                var query = _repository.GetProducts(q, page, pageSize);
                var pagedResult = new PagedResult<Product>(query, page, pageSize);
                var items = _mapper.Map<IList<ProductReadDto>>(pagedResult.Items);
                return new JsonResult(new {items, pagination = pagedResult.Pagination});
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }

        // GET /api/products/{productId}
        [HttpGet("{productId}")]
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

        [HttpGet("{productId}/all")]
        public ActionResult GetProductsByProductId(string productId,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var query = _repository.GetProductsById(productId, page, pageSize);
                var pagedResult = new PagedResult<Product>(query, page, pageSize);
                var items = _mapper.Map<IList<ProductReadDto>>(pagedResult.Items);
                return new JsonResult(new {items, pagination = pagedResult.Pagination});
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }

        [HttpGet("setSpiderSchedule")]
        public ActionResult SetSpiderSchedule()
        {
            try
            {
                if (Spider.SpiderMaker.HasJob)
                    return Content("Spider already have a job, it will update entire website at every 3 am. ");
                Spider.SpiderMaker.GetAJob();
                JobManager.AddJob(Spider.SpiderMaker.Crawl, (s) => s.ToRunEvery(1).Days().At(3, 0));
                return Content("Set schedules Successfully, the Spider will update entire website at every 3 am.");
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }
    }
}