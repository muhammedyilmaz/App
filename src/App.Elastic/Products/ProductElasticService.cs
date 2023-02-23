using App.Elastic;
using App.Elastic.Dto;
using App.Products.Dto;
using Microsoft.Extensions.Configuration;
using Nest;
using System;
using System.Collections.Generic;

namespace App.Products
{
    public class ProductElasticService : IProductElasticService
    {
        #region Fields

        private readonly IConfiguration _configuration;
        private readonly IElasticContext _elasticContext;

        #endregion

        #region Ctor

        public ProductElasticService(IConfiguration configuration,
            IElasticContext elasticContext)
        {
            _configuration = configuration;
            _elasticContext = elasticContext;
        }

        #endregion

        #region Utilities

        private IElasticResponse CreateIndex(ElasticClient client, string index)
        {
            var existsIndex = client.Indices.Exists(index);
            if (!existsIndex.Exists)
            {
                var response = client.Indices.Create(index,
                         index => index.Map<ProductElasticDto>(
                             x => x.AutoMap()
                         ));

                if (!response.IsValid)
                    return new ElasticResponse<ProductElasticService>(false, response.ServerError.Error.Reason, response.OriginalException);
            }

            return new ElasticResponse<ProductElasticService>(true, "Succesfully");
        }

        #endregion

        #region Methods

        public ElasticResponse<List<ProductElasticDto>> GetAll()
        {
            try
            {
                var index = _configuration.GetSection("Elastic:Index").Value;

                var client = _elasticContext.GetElasticClient(index);
                var createIndexResponse = CreateIndex(client, index);
                if (!createIndexResponse.IsSuccess)
                    return new ElasticResponse<List<ProductElasticDto>>(false, createIndexResponse.Message, createIndexResponse.Exception);

                var searchResponse = client.Search<ProductElasticDto>(x => x
                        .Query(y => y
                            .MatchAll()
                        )
                    );

                if (!searchResponse.IsValid)
                    return new ElasticResponse<List<ProductElasticDto>>(false, searchResponse.ServerError.Error.Reason, searchResponse.OriginalException);

                var products = new List<ProductElasticDto>();
                foreach (var item in searchResponse.Documents)
                {
                    products.Add(item);
                }

                return new ElasticResponse<List<ProductElasticDto>>(true, "Succesfully", products);
            }
            catch (Exception ex)
            {
                return new ElasticResponse<List<ProductElasticDto>>(false, ex.Message, ex);
            }
        }

        public ElasticResponse<List<ProductElasticDto>> GetAllByBarcode(string barcode)
        {
            try
            {
                var index = _configuration.GetSection("Elastic:Index").Value;

                var client = _elasticContext.GetElasticClient(index);
                var createIndexResponse = CreateIndex(client, index);
                if (!createIndexResponse.IsSuccess)
                    return new ElasticResponse<List<ProductElasticDto>>(false, createIndexResponse.Message, createIndexResponse.Exception);

                var searchResponse = client.Search<ProductElasticDto>(s => s
                                       .Query(q => q
                                           .Bool(b => b
                                               .Filter(bf => bf
                                                   .DateRange(r => r
                                                       .Field(f => f.Barcode)
                                                       .GreaterThanOrEquals(barcode)
                                                   )
                                               )
                                             )
                                           )
                                        );

                if (!searchResponse.IsValid)
                    return new ElasticResponse<List<ProductElasticDto>>(false, searchResponse.ServerError.Error.Reason, searchResponse.OriginalException);

                var products = new List<ProductElasticDto>();
                foreach (var item in searchResponse.Documents)
                {
                    products.Add(item);
                }

                return new ElasticResponse<List<ProductElasticDto>>(true, "Succesfully", products);
            }
            catch (Exception ex)
            {
                return new ElasticResponse<List<ProductElasticDto>>(false, ex.Message, ex);
            }
        }

        public ElasticResponse<bool> CreateAllData(List<ProductElasticDto> productElasticDtos)
        {
            try
            {
                var index = _configuration.GetSection("Elastic:Index").Value;

                var client = _elasticContext.GetElasticClient(index);
                var createIndexResponse = CreateIndex(client, index);
                if (!createIndexResponse.IsSuccess)
                    return new ElasticResponse<bool>(false, createIndexResponse.Message, createIndexResponse.Exception);

                foreach (var product in productElasticDtos)
                {
                    var indexResponse = client.IndexDocument(product);
                    if (!indexResponse.IsValid)
                        return new ElasticResponse<bool>(false, indexResponse.ServerError.Error.Reason, indexResponse.OriginalException);
                }

                return new ElasticResponse<bool>(true, "Succesfully", true);
            }
            catch (Exception ex)
            {
                return new ElasticResponse<bool>(false, ex.Message, ex);
            }
        }

        public ElasticResponse<bool> ClearIndex()
        {
            try
            {
                var index = _configuration.GetSection("Elastic:Index").Value;
                var client = _elasticContext.GetElasticClient(index);
                var response = client.Indices.Delete(index);

                if (!response.IsValid)
                    return new ElasticResponse<bool>(false, response.ServerError.Error.Reason, response.OriginalException);

                return new ElasticResponse<bool>(true, "Succesfully");
            }
            catch (Exception ex)
            {
                return new ElasticResponse<bool>(false, ex.Message, ex);
            }
        }

        #endregion
    }
}
