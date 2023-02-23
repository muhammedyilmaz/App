using App.Elastic.Dto;
using App.Products.Dto;
using System.Collections.Generic;

namespace App.Products
{
    public interface IProductElasticService
    {
        ElasticResponse<List<ProductElasticDto>> GetAll();
        ElasticResponse<List<ProductElasticDto>> GetAllByBarcode(string barcode);
        ElasticResponse<bool> CreateAllData(List<ProductElasticDto> productElasticDtos);
        ElasticResponse<bool> ClearIndex();
    }
}
