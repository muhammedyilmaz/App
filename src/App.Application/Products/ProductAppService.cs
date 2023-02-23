using App.Application.Services;
using App.Domain.Repositories;
using App.Products.Dto;
using System.Collections.Generic;

namespace App.Products
{
    public class ProductAppService : ApplicationService, IProductAppService
    {
        #region Fields

        private readonly IRepository<Product> _productRepository;

        #endregion

        #region Ctor

        public ProductAppService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        public List<ProductElasticDto> GetAllProductElasticDtoList()
        {
            var dboProducts = _productRepository.GetAllList();
            var products = new List<ProductElasticDto>();
            foreach (var item in dboProducts)
            {
                var product = new ProductElasticDto()
                {
                    Id = item.Id,
                    Barcode = item.Barcode,
                    Description = item.Description
                };

                products.Add(product);
            }

            return products;
        }

        #endregion
    }
}
