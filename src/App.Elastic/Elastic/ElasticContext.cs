using Microsoft.Extensions.Configuration;
using Elasticsearch.Net;
using Nest;
using System;

namespace App.Elastic
{
    public class ElasticContext : IElasticContext
    {
        #region Fields

        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public ElasticContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public ElasticClient GetElasticClient(string index)
        {
            #region Connection Info

            var url = _configuration.GetSection("Elastic:Url").Value;
            var userName = _configuration.GetSection("Elastic:UserName").Value;
            var password = _configuration.GetSection("Elastic:Password").Value;

            #endregion

            #region Connect

            var pool = new SingleNodeConnectionPool(new Uri(url));
            var connectionSettings = new ConnectionSettings(pool)
                    .BasicAuthentication(userName, password)
                    .DefaultIndex(index);

            var client = new ElasticClient(connectionSettings);
            return client;

            #endregion
        }

        #endregion
    }
}
