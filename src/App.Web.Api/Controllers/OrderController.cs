using App.Orders;
using App.Web.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using App.Orders.Dto;

namespace App.Web.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : AppApiControllerBase
    {
        #region Fields

        private readonly IOrderAppService _orderAppService;

        #endregion

        #region Ctor

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        #endregion

        #region Methods

        [HttpGet("GetAll")]
        public IActionResult GetAll(int orderId, string orderSource, string customerName)
        {
            try
            {
                var input = new OrderGetAllInput()
                {
                    OrderId = orderId,
                    OrderSource = orderSource,
                    CustomerName = customerName
                };

                var orders = _orderAppService.GetAll(input);
                return StatusCode(200, orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] OrderCreatedInput input)
        {
            try
            {
                _orderAppService.Create(input);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }

        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            try
            {
                _orderAppService.Delete(id);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPut("Edit")]
        public IActionResult Edit([FromBody] OrderUpdatedInput input)
        {
            try
            {
                _orderAppService.Update(input);
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        #endregion
    }
}
