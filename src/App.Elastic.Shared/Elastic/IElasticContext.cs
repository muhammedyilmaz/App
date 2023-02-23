using Nest;

namespace App.Elastic
{
    public interface IElasticContext
    {
        ElasticClient GetElasticClient(string index);
    }
}
