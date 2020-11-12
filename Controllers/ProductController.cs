using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Common;
using AutoMapper;
using Microsoft.AspNetCore.Http;
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
        public ActionResult GetProducts(string q = "", int page = 1, int pageSize = 10, string category = "",
            bool isPromotion = false)
        {
            q ??= "";
            category ??= "";
            try
            {
                var query = _repository.GetProducts(q, page, pageSize, category, isPromotion);
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

        // GET /api/products/{productId}/all
        [HttpGet("{productId}/all")]
        public async Task<ActionResult<ProductReadDto>> GetProductsByProductId(string productId,
            int page = 1,
            int pageSize = 10)
        {
            try
            {
                var query = await _repository.GetProductsById(productId, page, pageSize);
                var items = _mapper.Map<IList<ProductReadDto>>(query);
                return Ok(items);
            }
            catch (Exception error)
            {
                return BadRequest(MyUtils.ExceptionMessage(error));
            }
        }

        // GET /api/products/related
        [HttpGet("related")]
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
            return Content("The Spider already got a job!");
        }

        [HttpPost("test")]
        public async Task<ActionResult> Test()
        {
            // var q = obj.q !=null ? Convert.ToString(obj.q) : "";
            // var currentPage = obj.currentPage !=null ? Convert.ToInt32(obj.currentPage): 1;
            // var sort = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(Convert.ToString(obj.sort));
            // var perPage = obj.perPage != null ? Convert.ToInt32(obj.perPage) : 10 ;
            // var categoryId = obj.categoryId != null ? Convert.ToInt32(obj.categoryId) : 0;

            string rawValue = await GetRawBodyStringAsync(Request);
            var query = Newtonsoft.Json.JsonConvert.DeserializeObject<Query>(rawValue);
            return NoContent();
        }

        public static async Task<string> GetRawBodyStringAsync(HttpRequest request, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                return await reader.ReadToEndAsync();
        }

        class Query
        {
            public string q { get; set; }
            public Dictionary<string, int> sort { get; set; }
        }
    }
}