using App.Elastic;
using App.Products;
using Autofac;

namespace App
{
    public class ElasticModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ElasticContext>().As<IElasticContext>().InstancePerDependency();
            builder.RegisterType<ProductElasticService>().As<IProductElasticService>().InstancePerDependency();
        }
    }
}
