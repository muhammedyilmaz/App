using App.Application.Services;
using App.Products.Dto;
using System.Collections.Generic;

namespace App.Products
{
    public interface IProductAppService : IApplicationService
    {
        List<ProductElasticDto> GetAllProductElasticDtoList();
    }
}
