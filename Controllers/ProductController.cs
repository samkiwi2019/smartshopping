using System.Collections.Generic;
using System.Threading.Tasks;
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
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts(string q = "", int page = 1,
            int pageSize = 10)
        {
            var productItems = await _repository.GetProducts(q, page, pageSize);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }

        //5012874-EA-000PNS
        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductReadDto>> GetProductByProductId(string productId, string category = "")
        {
            var productItem = await _repository.GetProductById(productId, category);
            return Ok(_mapper.Map<ProductReadDto>(productItem));
        }

        [HttpGet("{productId}/all")]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProductsByProductId(string productId, int page = 1,
            int pageSize = 10)
        {
            var productItems = await _repository.GetProductsById(productId, page, pageSize);
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }


        [HttpGet("setSpiderSchedule")]
        public ActionResult SetSpiderSchedule()
        {
            if (Spider.SpiderMaker.HasJob)
                return Content("Spider already have a job, it will update entire website at every 3 am. ");
            Spider.SpiderMaker.GetAJob();
            JobManager.AddJob(Spider.SpiderMaker.Crawl, (s) => s.ToRunEvery(1).Days().At(3, 0));
            return Content("Set schedules Successfully, the Spider will update entire website at every 3 am.");
        }
    }
}