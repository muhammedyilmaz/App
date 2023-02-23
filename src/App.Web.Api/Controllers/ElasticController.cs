using App.Products;
using App.Web.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace App.Web.Controllers
{
    [Route("api/[controller]")]
    public class ElasticController : AppApiControllerBase
    {
        #region Fields

        private readonly IProductElasticService _productElasticService;
        private readonly IProductAppService _productAppService;

        #endregion

        #region Ctor

        public ElasticController(IProductElasticService productElasticService,
            IProductAppService productAppService)
        {
            _productElasticService = productElasticService;
            _productAppService = productAppService;
        }

        #endregion

        #region Methods

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _productElasticService.GetAll();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }


        [HttpGet("GetAllByBarcode")]
        public IActionResult GetAllByBarcode(string barcode)
        {
            var result = _productElasticService.GetAllByBarcode(barcode);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpPost("CreateAllData")]
        public IActionResult CreateAllData()
        {
            var product = _productAppService.GetAllProductElasticDtoList();
            if (product.Count == 0)
            {
                return Ok(200);
            }

            var result = _productElasticService.CreateAllData(product);
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        [HttpDelete("ClearIndex")]
        public IActionResult ClearIndex()
        {
            var result = _productElasticService.ClearIndex();
            if (result.IsSuccess)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        #endregion
    }
}
